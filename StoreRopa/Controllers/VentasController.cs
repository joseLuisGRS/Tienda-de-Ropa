using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.utils;
using StoreRopa.Models.Vo;

namespace StoreRopa.Controllers
{
    public class VentasController : Controller
    {
        private readonly ILogger<VentasController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public VentasController(ILogger<VentasController> logger, IUnitOfWork unitOfWork) { 
            this._logger = logger;
            this._unitOfWork = unitOfWork;
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
                if(tipo == 1) ModelState.Remove("Curp");
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
                if (ModelState.IsValid)
                {
                    Cliente client = await this._unitOfWork.ClientesRepository.GetById(venta.Venta.ClienteId);
                    if (client == null)
                    {
                        throw new CustomException("El cliente no existe.");
                    }
                    resultado = Constantes.EXITO;
                }
                ViewBag.Exito = Constantes.ERROR;
                //return PartialView("Clientes", personas);
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
                        
            return Content(string.Format("{0}:{1}", resultado,""));
        }

    }
}
