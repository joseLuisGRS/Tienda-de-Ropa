using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.Vo;

namespace StoreRopa.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CurrentUser _currentUser;
        private readonly User _user;
        public RolesController(ILogger<RolesController> logger, IUnitOfWork unitOfWork, CurrentUser currentUser) {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _user = _currentUser.Builder();
        }

        /// <summary>
        /// Método encargado de la obtención de roles que no han sido eliminados logicamente
        /// recibe como parametros lel tamaño de la paginación y número de página a consultar
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("/Roles")]
        public IActionResult GetRoles(int pageSize = 1, int page = 1)
        {
            try
            {
                if (pageSize == 1) pageSize = Constantes.PAGE_SIZE;
                _logger.LogInformation("Se realiza búsqueda de roles");                
                return View("Index", _unitOfWork.RolesRepository.GetAllRoles(pageSize, page).Result);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en la búsqueda de roles: " + e.Message);
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return View("Index");
            }
        }

        /// <summary>
        /// Método encargado de direccionar a la vista para el registro de roles
        /// </summary>
        /// <returns>Vista para el registro de roles</returns>
        public IActionResult Create()
        {
            return PartialView("Create", new RolesVO());
        }

        /// <summary>
        /// Método para el registro de roles
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RolesVO rolVO)
        {
            ViewBag.Exito = Constantes.ERROR;
            ModelState.Remove("roles.Empleados");
            if (ModelState.IsValid)
            {
                try
                {
                    string nombre = rolVO.roles.Nombre.Replace(" ", String.Empty);
                    rolVO.roles.Nombre = nombre;
                    Roles rolBd = await _unitOfWork.RolesRepository.getRolByName(nombre);
                    if (rolBd != null)
                    {
                        throw new CustomException("El nombre del rol ya se encuentra registrado.");
                    }
                    rolVO.roles.UsuarioAlta = _user.Id.ToString();
                    Roles rol = rolVO.roles;
                    await _unitOfWork.RolesRepository.Create(rol);
                    _unitOfWork.SaveChangesAsync().Wait();
                    _logger.LogInformation("Se realiza registro en BD del rol con id " + rol.Id);
                    ViewBag.Exito = Constantes.EXITO;
                    return PartialView("Create", new RolesVO());
                }
                catch (CustomException e)
                {
                    _logger.LogError("Se presento error en el registro de roles: " + e.Message);
                    ViewData["error"] = e.Message;
                }
                catch (AggregateException e)
                {
                    _logger.LogError("Se presento error en el registro de roles: " + e.Message);
                    ViewData["error"] = Messages.ERROR_MESSAGE;
                }
                catch (Exception e)
                {
                    _logger.LogCritical("Se presento error en el registro de roles: " + e.Message);
                    ViewData["error"] = Messages.ERROR_MESSAGE;
                }
            }
            return PartialView("Create", new RolesVO());
        }

        /// <summary>
        /// Método que carga la vista con los datos del rol que se modificará
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Int64? id)
        {
            try
            {
                if (id == null) throw new CustomException("Rol no encontrado.");
                var rol = await _unitOfWork.RolesRepository.GetById((long) id);
                if (rol == null) throw new CustomException("Rol no encontrado.");
                ViewBag.Exito = Constantes.EXITO;
                return PartialView("Edit", new RolesVO(rol));
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error en la edición del rol id " + id + ": " + e.Message);
                ViewData["error"] = e.Message;
                return PartialView("Edit", null);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en la edición del rol id " + id + ": " + e.Message);
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return PartialView("Edit", null);
            }
        }

        /// <summary>
        /// Método encargado de la actualización del rol
        /// </summary>
        /// <param name="rolesVO"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RolesVO rolesVO)
        {
            ViewBag.Exito = Constantes.ERROR;
            ModelState.Remove("roles.Empleados");
            if (ModelState.IsValid)
            {
                try
                {
                    Roles rol = await _unitOfWork.RolesRepository.GetById(rolesVO.roles.Id);
                    if (rol == null)
                    {
                        throw new CustomException("El rol no existe.");
                    }
                    string nombre = rolesVO.roles.Nombre.Replace(" ", String.Empty);
                    rolesVO.roles.Nombre = nombre;
                    Roles rolName = await _unitOfWork.RolesRepository.getRolByName(nombre);
                    if (rolName != null && rolName.Id != rolesVO.roles.Id)
                    {
                        throw new CustomException("El rol ya se encuentra registrado.");
                    }
                    rol.UsuarioModificacion = _user.Id.ToString();
                    rol.Nombre = rolesVO.roles.Nombre;
                    rol.Descripcion = rolesVO.roles.Descripcion;
                    _unitOfWork.RolesRepository.Update(rol);
                    _unitOfWork.SaveChangesAsync().Wait();
                    _logger.LogInformation("Se realiza la actualización del rol con id: " + rol.Id);
                    ViewBag.Exito = Constantes.EXITO;
                    return PartialView("Edit", new RolesVO());
                }
                catch (CustomException e)
                {
                    _logger.LogError("Se presento un error en la edición del rol: " + e.Message);
                    ViewData["error"] = e.Message;
                }
                catch (Exception e)
                {
                    _logger.LogCritical("Se presento un error en la edición del rol: " + e.Message);
                    ViewData["error"] = Messages.ERROR_MESSAGE;
                }
            }
            return PartialView("Edit", new RolesVO());
        }

        /// <summary>
        /// Método encargado del cambio de estatus de roles
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Status(Int64? id)
        {
            int resultado = Constantes.ERROR;
            try
            {
                Roles rol = await _unitOfWork.RolesRepository.GetById((long)id!);
                if (rol == null)
                {
                    throw new CustomException("El rol no existe.");
                }
                Empleados empleado = await _unitOfWork.EmpleadosRepository
                    .GetEmpleadoRolByRolId(rol.Id);
                if (empleado != null && rol.EsActivo)
                {
                    return Content(string.Format("{0}", 2));
                }
                string opcion = "re-activación";
                if (rol.EsActivo) opcion = "desactiva";
                rol.UsuarioModificacion = _user.Id.ToString();
                _unitOfWork.RolesRepository.UpdateEstatus(rol);
                _unitOfWork.SaveChangesAsync().Wait();
                _logger.LogInformation("Se realiza cambio de estatus (" + opcion + ") al rol con id: " + rol.Id);
                resultado = Constantes.EXITO;
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error al cambiar estatus de rol con id " + id + ": " + e.Message);
                return Content(string.Format("{0}", resultado));
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error al cambiar estatus de rol con id " + id + ": " + e.Message);
                return Content(string.Format("{0}", resultado));
            }
            return Content(string.Format("{0}", resultado));
        }

        /// <summary>
        /// Método encargado de la eliminación lógica del rol
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Int64? id)
        {
            int resultado = Constantes.ERROR;
            try
            {
                Roles rol = await _unitOfWork.RolesRepository.GetById((long)id!);
                if (rol == null)
                {
                    throw new CustomException("El rol no existe.");
                }
                Empleados empleado = await _unitOfWork.EmpleadosRepository.GetEmpleadoRolByRolId(rol.Id);
                if (empleado != null && rol.EsActivo)
                {
                    return Content(string.Format("{0}", 2));
                }               
                rol.UsuarioModificacion = _user.Id.ToString();
                _unitOfWork.RolesRepository.Delete(rol);
                _unitOfWork.SaveChangesAsync().Wait();
                _logger.LogInformation("Se da de baja el rol con id " + rol.Id);
                resultado = Constantes.EXITO;
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error al dar de baja el rol con id " + id);
                return Content(string.Format("{0}", resultado));
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error al dar de baja el rol con id " + id);
                return Content(string.Format("{0}", resultado));
            }
            return Content(string.Format("{0}", resultado));
        }

    }
}
