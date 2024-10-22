using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.utils;
using StoreRopa.Models.Vo;

namespace StoreRopa.Controllers
{
    [Authorize]
    public class VentasController : Controller
    {
        private readonly ILogger<VentasController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly CurrentUser _currentUser;
        private readonly User _user;
        private readonly UserManager<ApplicationUser> _userManager;
        private decimal _abonoInicial = 0;
        private decimal _abono = 0;
        private decimal _abonoRecibido = 0;
        private decimal _abonoTotal = 0;
        public VentasController(ILogger<VentasController> logger, IUnitOfWork unitOfWork, IConfiguration configuration,
            CurrentUser currentUser) {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _currentUser = currentUser;
            _user = _currentUser.Builder();
        }

        /// <summary>
        /// Método principal para cargar la pantalla de ventas
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            ViewData["Clientes"] = _unitOfWork.ClientesRepository.GetClientesPersona().Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Persona.Nombres + " " + e.Persona.ApPaterno + " " + e.Persona.ApMaterno
            }).ToList();
            ViewBag.Exito = "-1";
            return View(new VentasVO());
        }

        /// <summary>
        /// Método que consulta las coincidencias de los clientes
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="curp"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IActionResult> ConsultarCliente(int tipo, Int64 id, string Curp, int pageSize = 1, int page = 1)
        {
            try
            {
                PagedResult<Persona> personas = new PagedResult<Persona>();
                if (tipo == 1) ModelState.Remove("Curp");
                else ModelState.Remove("id");
                if (ModelState.IsValid)
                {
                    if (pageSize == 1) pageSize = Constantes.PAGE_SIZE;
                    _logger.LogInformation("Se realiza búsqueda de clientes.");
                    personas = this._unitOfWork.PersonasRepository.GetClientesByCoincidencia(pageSize, page, id, Curp, tipo).Result;
                    ViewBag.Exito = Constantes.EXITO;
                    return PartialView("Clientes", personas);
                }
                ViewBag.Exito = Constantes.ERROR;
                return PartialView("Clientes", personas);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en la búsqueda de clientes: " + e.Message);
                ViewBag.Exito = Constantes.ERROR;
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return PartialView("Clientes", new PagedResult<Persona>());
            }
        }

        public int ConsultarTipoVentaClienteById(Int64 id)
        {
            int tipoVenta = 0;
            try
            {
                Cliente cliente = new Cliente();
                _logger.LogInformation("Se realiza búsqueda de cliente con id: " + id);
                cliente = this._unitOfWork.ClientesRepository.GetById(id).Result;
                ViewBag.Exito = Constantes.EXITO;
                return (int)cliente.TipoVenta;
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en la búsqueda de cliente con id " + id + " " + e.Message);
                ViewBag.Exito = Constantes.ERROR;
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return tipoVenta;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerarVenta(GeneraVentaVO venta)
        {
            int resultado = Constantes.ERROR;
            try
            {
                ModelState.Remove("venta.Venta.Cliente");
                ModelState.Remove("venta.Venta.Empleado");
                ModelState.Remove("venta.Venta.DetalleVentas");
                for (int i = 0; i < venta.DetallesDeVentas!.Count; i++)
                {
                    ModelState.Remove("venta.DetallesDeVentas[" + i + "].Venta");
                    ModelState.Remove("venta.DetallesDeVentas[" + i + "].Creditos");
                }

                if (ModelState.IsValid)
                {
                    Cliente client = await this._unitOfWork.ClientesRepository.GetById(venta.Venta.ClienteId);
                    if (client == null)
                    {
                        throw new CustomException("El cliente no existe.");
                    }
                    Ventas newVenta = new Ventas();
                    newVenta.ClienteId = venta.Venta.ClienteId;
                    newVenta.EmpleadoId = _user.Id;
                    newVenta.ImporteVenta = venta.Venta.ImporteVenta;
                    newVenta.AbonoVenta = venta.Venta.AbonoVenta;
                    newVenta.PendientePago = (decimal)(venta.Venta?.PendientePago);
                    newVenta.EsVentaCredito = venta.Venta.EsVentaCredito;
                    newVenta.UsuarioAlta = _user.Id.ToString();
                    await _unitOfWork.VentaRepository.Create(newVenta);
                    List<DetalleVentasVo> detalleVentasVoList = new List<DetalleVentasVo>();
                    venta.DetallesDeVentas.ForEach(det => {
                        DetalleVentasVo detalleVentasVo = new DetalleVentasVo(det);
                        detalleVentasVoList.Add(detalleVentasVo);
                    });
                    detalleVentasVoList.Sort((det1, det2) => det1.DetalleVentas.PrecioVenta.CompareTo(det2.DetalleVentas.PrecioVenta));
                    if (newVenta.EsVentaCredito && newVenta.AbonoVenta > 0)
                    {
                        _abono = newVenta.AbonoVenta / detalleVentasVoList.Count();
                        _abonoInicial = _abono;
                        _abonoTotal = newVenta.AbonoVenta;
                        for (int i = 0; i < detalleVentasVoList.Count(); i++)
                        {
                            bool esUltimo = i == detalleVentasVoList.Count() - 1 ? true : false;
                            detalleVentasVoList[i].abonoArticulo = calculaAbono(esUltimo, detalleVentasVoList[i].DetalleVentas.PrecioVenta);
                        }
                    }
                    for(int i = 0;i<detalleVentasVoList.Count();i++)
                    {
                        DetalleVentas detalle = detalleVentasVoList[i].DetalleVentas;
                        detalle.VentaId = newVenta.Id;
                        detalle.Venta = newVenta;
                        detalle.UsuarioAlta = _user.Id.ToString();
                        await _unitOfWork.DetalleVentaRepository.Create(detalle);
                        if (newVenta.EsVentaCredito)
                        {
                            Creditos credito = new Creditos();
                            credito.DetalleVentaId = detalle.Id;
                            credito.DetalleVenta = detalle;
                            credito.PrecioArticulo = detalle.PrecioVenta;
                            credito.PagoPendiente = decimal.Parse((detalle.PrecioVenta - detalleVentasVoList[i].abonoArticulo)
                                .ToString("0.00"));
                            credito.UsuarioAlta = _user.Id.ToString();
                            await _unitOfWork.CreditosRepository.Create(credito);
                        }
                    }
                    _unitOfWork.SaveChangesAsync().Wait();
                    _logger.LogInformation("Se realiza registro en BD de la Venta con id " + newVenta.Id);
                    try
                    {
                        venta.Venta = newVenta;
                        venta.Venta.Cliente = client;
                        Ticket ticket = new Ticket(_unitOfWork,_configuration, 15, true, venta);
                        ticket.ImprimirTicket();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al imprimir el ticket: {ex.Message}");
                    }
                    resultado = Constantes.EXITO;
                }
                ViewBag.Exito = Constantes.ERROR;
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error al realizar venta al cliente " + venta.Venta.ClienteId + ": " + e.Message);
                return Content(string.Format("{0}:{1}", resultado, e.Message));
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error al realizar venta al cliente " + venta.Venta.ClienteId + ": " + e.Message);
                return Content(string.Format("{0}:{1}", resultado, Messages.ERROR_MESSAGE));
            }

            return Content(string.Format("{0}:{1}", resultado, ""));
        }

        private decimal calculaAbono(bool esUltimo, decimal precio) 
        { 
            if (!esUltimo)
            {
                if (precio >= _abono) 
                {
                    _abonoRecibido += decimal.Parse(_abono.ToString("0.00"));
                    decimal abonoArticulo = decimal.Parse(_abono.ToString("0.00"));
                    if (_abono != _abonoInicial) _abono = _abonoInicial;
                    return abonoArticulo;
                }
                else
                {
                    _abonoRecibido += decimal.Parse(precio.ToString("0.00"));
                    _abono = _abonoInicial + (_abono - precio);
                    return decimal.Parse(precio.ToString("0.00"));
                }
            }
            else
            {
                return decimal.Parse((_abonoTotal - _abonoRecibido).ToString("0.00"));
            }

        }
    }
}
