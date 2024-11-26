using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.Vo;

namespace StoreRopa.Controllers
{
    [Authorize]
    public class AbonosController : Controller
    {
        private readonly ILogger<AbonosController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly CurrentUser _currentUser;
        private readonly User _user;

        public AbonosController(ILogger<AbonosController> logger, IUnitOfWork unitOfWork, IConfiguration configuration,
            CurrentUser currentUser) 
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _currentUser = currentUser;
            _user = _currentUser.Builder();
        }
        public IActionResult Index()
        {
            ViewData["Clientes"] = _unitOfWork.ClientesRepository.GetClientesPersona().Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Persona.Nombres + " " + e.Persona.ApPaterno + " " + e.Persona.ApMaterno
            }).ToList();
            ViewBag.Exito = "-1";
            return View(new AbonoVO());
        }

        public IActionResult GetVentasCreditoById(Int64 id, int pageSize = 1, int page = 1)
        {
            try
            {
                if (id == null) throw new CustomException("Cliente no encontrado.");
                _logger.LogInformation("Se realiza búsqueda de cliente con id: " + id);
                var cliente = _unitOfWork.ClientesRepository.GetById(id).Result;
                if (cliente == null) throw new CustomException("Cliente no encontrado.");
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                };
                var ventasACredito = _unitOfWork.VentaRepository.GetVentasCreditoById(id).ToList();
                var json = JsonConvert.SerializeObject(ventasACredito,settings);
                return Content(json, "application/json");

            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en la búsqueda de cliente con id " + id + " " + e.Message);
                ViewBag.Exito = Constantes.ERROR;
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return View("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerarAbono(AbonoVO abonoVO) {
            int resultado = Constantes.ERROR;
            try
            {
                ModelState.Remove("abonoVo.Curp");
                ModelState.Remove("abonoVo.ClienteId");
                ModelState.Remove("abonoVo.ClaveCliente");
                ModelState.Remove("abonoVo.TipoBusqueda");
                ModelState.Remove("abonoVo.NombreCliente");
                if (ModelState.IsValid)
                {
                    Cliente client = await _unitOfWork.ClientesRepository.GetById(abonoVO.AbonoClienteId);
                    if (client == null)
                    {
                        throw new CustomException("El cliente no existe.");
                    }
                    var ventasACredio = _unitOfWork.VentaRepository.GetVentasCreditoById(abonoVO.AbonoClienteId).ToList();
                    var abonoGral = abonoVO.Abono;
                    TicketAbonoVO ticketAbonoVO = new TicketAbonoVO();
                    List<DetalleAbonoVO> detalleAbonoVOs = new List<DetalleAbonoVO>();
                    foreach (Ventas venta in ventasACredio) {
                        if (abonoGral == 0) break;
                        decimal abonadoAVenta = 0;
                        foreach(DetalleVentas detalleVentas in venta.DetalleVentas)
                        {
                            var credito = _unitOfWork.CreditosRepository.GetCreditoByDetalleVentaId(detalleVentas.Id);
                            if (credito != null) {
                                if (credito.PagoPendiente > 0) {
                                    string articulo = detalleVentas.Modelo + " " + detalleVentas.Descripcion + " " + detalleVentas.Color 
                                        + " T." + detalleVentas.Talla;
                                    DetalleAbonoVO detalleAbonoVO = new DetalleAbonoVO()
                                    {
                                        Articulo = articulo,
                                        VentaId = detalleVentas.VentaId,
                                    };
                                    if (credito.PagoPendiente >= abonoGral)
                                    {
                                        detalleAbonoVO.Abono = abonoGral;
                                        detalleAbonoVOs.Add(detalleAbonoVO);
                                        abonadoAVenta += abonoGral;
                                        Abonos abono = new Abonos()
                                        {
                                            CreditoId = credito.Id,
                                            Credito = credito,
                                            Abono = abonoGral,
                                            UsuarioAlta = _user.Id.ToString()
                                        };
                                        await _unitOfWork.AbonosRepository.Create(abono);
                                        credito.PagoPendiente = (credito.PagoPendiente - abonoGral);
                                        credito.UsuarioModificacion = _user.Id.ToString();
                                        _unitOfWork.CreditosRepository.Update(credito);
                                        abonoGral = 0;
                                        _logger.LogInformation("Se realiza abono de manera exitosa para id cliente: " + abonoVO.AbonoClienteId);
                                        break;
                                    }
                                    else
                                    {
                                        detalleAbonoVO.Abono = credito.PagoPendiente;
                                        detalleAbonoVOs.Add(detalleAbonoVO);
                                        abonadoAVenta += credito.PagoPendiente;
                                        Abonos abono = new Abonos()
                                        {
                                            CreditoId = credito.Id,
                                            Credito = credito,
                                            Abono = credito.PagoPendiente,
                                            UsuarioAlta = _user.Id.ToString()
                                        };
                                        await _unitOfWork.AbonosRepository.Create(abono);
                                        abonoGral = abonoGral - credito.PagoPendiente;
                                        credito.PagoPendiente = 0;
                                        credito.UsuarioModificacion = _user.Id.ToString();
                                        _unitOfWork.CreditosRepository.Update(credito);
                                        _logger.LogInformation("Se realiza abono de manera exitosa para id cliente: " + abonoVO.AbonoClienteId);
                                    }
                                }
                            }
                        }
                        venta.AbonoVenta = (venta.AbonoVenta + abonadoAVenta);
                        venta.PendientePago = (venta.ImporteVenta - venta.AbonoVenta);
                        venta.UsuarioModificacion = _user.Id.ToString();
                        _unitOfWork.VentaRepository.Update(venta);
                        _logger.LogInformation("Se realiza abono por (" + abonadoAVenta + ") de manera exitosa para id cliente: " 
                            + abonoVO.AbonoClienteId + " a la venta: " + venta.Id);                       
                    }
                    ticketAbonoVO.FechaAbono = DateTime.Now;
                    ticketAbonoVO.UsuarioAlta = _user.Id.ToString();
                    ticketAbonoVO.ClienteId = abonoVO.AbonoClienteId;
                    ticketAbonoVO.DetalleAbonos = detalleAbonoVOs;
                    ticketAbonoVO.TotalAbono = abonoVO.Abono;
                    ticketAbonoVO.SaldoActual = abonoVO.Saldo;
                    ticketAbonoVO.Efectivo = abonoVO.CantidadRecibida;

                    client.Saldo = client.Saldo - abonoVO.Abono;
                    client.UsuarioModificacion = _user.Id.ToString();
                    _unitOfWork.ClientesRepository.Update(client);
                    _unitOfWork.SaveChangesAsync().Wait();
                    try
                    {
                        TicketPago ticketPago = new TicketPago(_unitOfWork, _configuration, 15, true, ticketAbonoVO);
                        ticketPago.ImprimirTicket();
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
                _logger.LogError("Se presento un error al realizar el abono al cliente " + abonoVO.AbonoClienteId + ": " + e.Message);
                return Content(string.Format("{0}:{1}", resultado, e.Message));
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error al realizar el abono al cliente " + abonoVO.AbonoClienteId + ": " + e.Message);
                return Content(string.Format("{0}:{1}", resultado, Messages.ERROR_MESSAGE));
            }

            return Content(string.Format("{0}:{1}", resultado, ""));
        }
    }
}
