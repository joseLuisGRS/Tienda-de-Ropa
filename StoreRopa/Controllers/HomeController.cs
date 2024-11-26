using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.Vo;
using System.Diagnostics;

namespace StoreRopa.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CurrentUser _currentUser;
        private readonly User _user;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, CurrentUser currentUser)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _user = _currentUser.Builder();
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> DetallesVentasAsync()
        {
            DetalleVentaDelDiaVO detalleVentaDelDia = new DetalleVentaDelDiaVO();
            detalleVentaDelDia.RolUsuario = _user.RolName;
            try
            {                
                _logger.LogInformation("Se realiza la analisis de ventas del día del usuario: " + _user.Id);
                Int64 id = _user.Id;
                DatosVentaAbonoVO datosVenta = await _unitOfWork.VentaRepository.GetTotalesVentasById(id);
                DatosVentaAbonoVO datosAbono = await _unitOfWork.AbonosRepository.GetTotalesAbonosById(id);

                detalleVentaDelDia.Ventas = datosVenta.Cantidad;
                detalleVentaDelDia.ImporteVenta = datosVenta.Importe;
                detalleVentaDelDia.Abonos = datosAbono.Cantidad;
                detalleVentaDelDia.ImporteAbonos = datosAbono.Importe;
                detalleVentaDelDia.ImporteTotal = detalleVentaDelDia.ImporteVenta + detalleVentaDelDia.ImporteAbonos;
                if (_user.RolName.ToLower() == Constantes.ADMININISTRADOR.ToLower()) id = 0;
                DatosVentaAbonoVO datosVentaG = await _unitOfWork.VentaRepository.GetTotalesVentasById(id);
                DatosVentaAbonoVO datosAbonoG = await _unitOfWork.AbonosRepository.GetTotalesAbonosById(id);
                detalleVentaDelDia.VentasG = datosVentaG.Cantidad;
                detalleVentaDelDia.ImporteVentaG = datosVentaG.Importe;
                detalleVentaDelDia.AbonosG = datosAbonoG.Cantidad;
                detalleVentaDelDia.ImporteAbonosG = datosAbonoG.Importe;
                detalleVentaDelDia.ImporteTotalG = detalleVentaDelDia.ImporteVentaG + detalleVentaDelDia.ImporteAbonosG;

                return Json(detalleVentaDelDia);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en analisis de ventas del día : " + e.Message);
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return Json(detalleVentaDelDia);
            }
        }
    }
}