using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Models;
using StoreRopa.Models.Vo;

namespace StoreRopa.Controllers
{
    [Authorize]
    public class DevolucionesController : Controller
    {
        private readonly ILogger<DevolucionesController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly CurrentUser _currentUser;
        private readonly User _user;
        public DevolucionesController(ILogger<DevolucionesController> logger, IUnitOfWork unitOfWork, IConfiguration configuration,
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
            return View(new VentasVO());
        }
    }
}
