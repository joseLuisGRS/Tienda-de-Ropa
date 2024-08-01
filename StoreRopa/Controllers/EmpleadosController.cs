using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.utils;
using StoreRopa.Models.Vo;

namespace StoreRopa.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private readonly ILogger<EmpleadosController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CurrentUser _currentUser;
        private readonly User _user;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmpleadosController(ILogger<EmpleadosController> logger, IUnitOfWork unitOfWork,
            CurrentUser currentUser, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _user = _currentUser.Builder();
            _userManager = userManager;
        }

        /// <summary>
        /// Método encargado de la obtención de personas, empleados, roles que no han sido eliminados logicamente
        /// recibe como parametros lel tamaño de la paginación y número de página a consultar
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("/Empleados")]
        public IActionResult GetEmpleados(int pageSize = 1, int page = 1)
        {
            try
            {
                if (pageSize == 1) pageSize = Constantes.PAGE_SIZE;
                _logger.LogInformation("Se realiza la búsqueda de empleados");
                var model = (_unitOfWork.EmpleadosRepository.GetEmpleados(pageSize, page).Result, _user);
                return View("Index", model);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en la búsqueda de empleados: " + e.Message);
                ViewData["error"] = Messages.ERROR_MESSAGE;
                PagedResult<Empleados> empleados = new PagedResult<Empleados>();
                var model = ( empleados, _user);
                return View("Index", model);
            }
        }

        /// <summary>
        /// Método encargado de direccionar a la vista para el registro de empleados
        /// </summary>
        /// <returns>Vista para el registro de empleados</returns>
        public IActionResult Create()
        {
            List<Roles> roles = _unitOfWork.RolesRepository.getRoles();
            return PartialView("Create", new PersonaEmpleadoVO(roles));
        }

        /// <summary>
        /// Método para el registro de personas y empleados
        /// </summary>
        /// <param name="personaEmpleado"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonaEmpleadoVO personaEmpleado)
        {
            ViewBag.Exito = Constantes.ERROR;
            ModelState.Remove("roles");
            ModelState.Remove("empleado.Rol");
            ModelState.Remove("empleado.Persona");
            ModelState.Remove("Persona.Cliente");
            ModelState.Remove("Persona.Empleado");
            ModelState.Remove("empleado.Ventas");
            if (ModelState.IsValid)
            {
                try
                {
                    if (!Int64.TryParse(personaEmpleado.persona.Telefono, out Int64 tel))
                    {
                        throw new FormatException("Verifique el teléfono.");
                    }
                    Persona personCurp = await _unitOfWork.PersonasRepository
                        .getPersonaByCurp(personaEmpleado.persona.Curp);
                    if (personCurp != null)
                    {
                        throw new CustomException("La curp ya se encuentra registrada.");
                    }
                    string user = personaEmpleado.empleado.Usuario.Replace(" ", String.Empty);
                    personaEmpleado.empleado.Usuario = user;
                    Empleados empleadoBD = await _unitOfWork.EmpleadosRepository
                        .GetEmpleadoByUser(personaEmpleado.empleado.Usuario);
                    if (empleadoBD != null)
                    {
                        throw new CustomException("Usuario invalido!, ya encuentra registrado.");
                    }
                    var pwdUnHashed = personaEmpleado.empleado.Password;
                    personaEmpleado.empleado.Password = Encrypt.GetSHA256(personaEmpleado.empleado.Password);
                    personaEmpleado.ConfirmaPwd = Encrypt.GetSHA256(personaEmpleado.ConfirmaPwd);
                    if (personaEmpleado.empleado.Password != personaEmpleado.ConfirmaPwd)
                    {
                        throw new CustomException("Las contraseñas no coinciden.");
                    }
                    personaEmpleado.persona.UsuarioAlta = _user.Id.ToString();
                    Persona persona = personaEmpleado.persona;
                    Empleados empleado = personaEmpleado.empleado;
                    await _unitOfWork.PersonasRepository.Create(persona);
                    empleado.UsuarioAlta = _user.Id.ToString();
                    empleado.Persona = persona;
                    empleado.RolId = (long)personaEmpleado.RolId!;
                    await _unitOfWork.EmpleadosRepository.Create(empleado);
                    var fullName = persona.Nombres + " " + persona.ApPaterno + " " + persona.ApMaterno;
                    var userIdentity = new ApplicationUser { UserName = empleado.Usuario, Email = "", FullName = fullName };
                    var result = await _userManager.CreateAsync(userIdentity, pwdUnHashed);
                    if (!result.Succeeded) {
                        throw new CustomException(result.Errors.First().Description);
                    }
                    _unitOfWork.SaveChangesAsync().Wait();
                    _logger.LogInformation("Se realiza registro en BD de la persona con id " + persona.Id);
                    _logger.LogInformation("Se realiza registro en BD del empleado con id " + empleado.Id);
                    ViewBag.Exito = Constantes.EXITO;
                    List<Roles> ListRoles = _unitOfWork.RolesRepository.getRoles();
                    return PartialView("Create", new PersonaEmpleadoVO(ListRoles));
                }
                catch (FormatException e)
                {
                    _logger.LogWarning("Se presento error de tipo de dato: " + e.Message);
                    ViewData["error"] = e.Message;
                }
                catch (CustomException e)
                {
                    _logger.LogError("Se presento error en el registro de empleado: " + e.Message);
                    ViewData["error"] = e.Message;
                }
                catch (AggregateException e)
                {
                    _logger.LogError("Se presento error en el registro de empleado: " + e.Message);
                    ViewData["error"] = Messages.ERROR_MESSAGE;
                }
                catch (Exception e)
                {
                    _logger.LogCritical("Se presento error en el registro de empleado: " + e.Message);
                    ViewData["error"] = Messages.ERROR_MESSAGE;
                }
            }
            List<Roles> roles = _unitOfWork.RolesRepository.getRoles();
            personaEmpleado.roles = roles;
            return PartialView("Create", personaEmpleado);
        }

        /// <summary>
        /// Método para mostrar el detalle de la persona y empleado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Detail(Int64? id)
        {
            try
            {
                if (id == null) throw new CustomException("Empleado no encontrado.");
                var persona = await _unitOfWork.EmpleadosRepository.getPersonaEmpleadoByIdPersona(id);
                if (persona == null) throw new CustomException("Empleado no encontrado.");
                return PartialView("Detail", persona);
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error al buscar persona (cliente) con id " + id + ": " + e.Message);
                ViewData["error"] = e.Message;
                return PartialView("Detail", null);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error al buscar persona (cliente) con id " + id + ": " + e.Message);
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return PartialView("Detail", null);
            }
        }

        /// <summary>
        /// Método que carga la vista con los datos de persona y empleado que se modificarán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Int64? id)
        {
            try
            {
                if (id == null) throw new CustomException("Empleado no encontrado.");
                var persona = await _unitOfWork.EmpleadosRepository.getPersonaEmpleadoByIdPersona(id);
                if (persona == null) throw new CustomException("Empleado no encontrado.");
                List<Roles> roles = _unitOfWork.RolesRepository.getRoles();
                return PartialView("Edit", new PersonaEmpleadoVO(persona.Persona, persona,
                    persona.RolId, roles));
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error en la edición del empleado idPersona " + id + ": " 
                    + e.Message);
                ViewData["error"] = e.Message;
                return PartialView("Edit", null);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en la edición del empleado idPersona " + id + ": " 
                    + e.Message);
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return PartialView("Edit", null);
            }
        }

        /// <summary>
        /// Método encargado de la actualización del empleado y persona
        /// </summary>
        /// <param name="personaEmpleado"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PersonaEmpleadoVO personaEmpleado)
        {
            ViewBag.Exito = Constantes.ERROR;
            ModelState.Remove("roles");
            ModelState.Remove("persona.Empleado.Rol");
            ModelState.Remove("persona.Empleado.Persona");
            ModelState.Remove("empleado.Rol");
            ModelState.Remove("empleado.Persona");
            ModelState.Remove("persona.Cliente");
            ModelState.Remove("persona.Empleado.Usuario");
            ModelState.Remove("persona.Empleado.Password");
            ModelState.Remove("empleado.Ventas");
            ModelState.Remove("persona.Empleado.Ventas");
            if (personaEmpleado.empleado.Password == null && personaEmpleado.ConfirmaPwd == null) {
                ModelState.Remove("empleado.Password");
                ModelState.Remove("ConfirmaPwd");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    Persona person = await _unitOfWork.PersonasRepository.GetById(personaEmpleado.persona.Id);
                    if (person == null)
                    {
                        throw new CustomException("El empleado no existe.");
                    }
                    Empleados oEmpleado = await _unitOfWork.EmpleadosRepository
                        .GetById(personaEmpleado.persona.Empleado.Id);
                    if (oEmpleado == null)
                    {
                        throw new CustomException("El empleado no existe.");
                    }
                    if (!Int64.TryParse(personaEmpleado.persona.Telefono, out Int64 tel))
                    {
                        throw new FormatException("Verifique el teléfono.");
                    }
                    Persona personCurp = await _unitOfWork.PersonasRepository
                        .getPersonaByCurp(personaEmpleado.persona.Curp);
                    if (personCurp != null && personCurp.Id != personaEmpleado.persona.Id)
                    {
                        throw new CustomException("La curp ya se encuentra registrada.");
                    }
                    string user = personaEmpleado.empleado.Usuario.Replace(" ", String.Empty);
                    personaEmpleado.empleado.Usuario = user;
                    Empleados empleadoBD = await _unitOfWork.EmpleadosRepository
                        .GetEmpleadoByUser(personaEmpleado.empleado.Usuario);
                    if (empleadoBD != null && empleadoBD.PersonaId != personaEmpleado.persona.Id)
                    {
                        throw new CustomException("Usuario invalido!, ya encuentra registrado.");
                    }                       
                    personaEmpleado.persona.UsuarioModificacion = _user.Id.ToString();
                    personaEmpleado.persona.FechaAlta = person.FechaAlta;
                    personaEmpleado.persona.UsuarioAlta = person.UsuarioAlta;
                    personaEmpleado.persona.EsActivo = person.EsActivo;
                    Persona persona = personaEmpleado.persona;
                    Empleados empleado = personaEmpleado.empleado;
                    bool isChangePwd = false;
                    string pwdUnHashed = "";
                    if (personaEmpleado.empleado.Password != null && personaEmpleado.ConfirmaPwd != null)
                    {
                        pwdUnHashed = personaEmpleado.empleado.Password;
                        personaEmpleado.empleado.Password = Encrypt.GetSHA256(personaEmpleado.empleado.Password);
                        personaEmpleado.ConfirmaPwd = Encrypt.GetSHA256(personaEmpleado.ConfirmaPwd);
                        if (personaEmpleado.empleado.Password != personaEmpleado.ConfirmaPwd)
                        {
                            throw new CustomException("Las contraseñas no coinciden.");
                        }
                        isChangePwd = true;
                    }
                    else {
                        empleado.Password = oEmpleado.Password;
                    }                  

                    _unitOfWork.PersonasRepository.Update(persona);
                    empleado.UsuarioModificacion = _user.Id.ToString();
                    empleado.FechaAlta = oEmpleado.FechaAlta;
                    empleado.UsuarioAlta = oEmpleado.UsuarioAlta;
                    empleado.EsActivo = oEmpleado.EsActivo;
                    empleado.Persona = persona;
                    empleado.RolId = (long)personaEmpleado.RolId!;
                    empleado.Id = oEmpleado.Id;
                    _unitOfWork.EmpleadosRepository.Update(empleado);

                    var userIdentity = await _userManager.FindByNameAsync(personaEmpleado.empleado.Usuario);
                    if (userIdentity == null)
                    {
                        throw new CustomException("El empleado no existe.");
                    }
                    userIdentity.UserName = personaEmpleado.empleado.Usuario;
                    userIdentity.FullName = personaEmpleado.persona.Nombres + " " +
                        personaEmpleado.persona.ApPaterno + " " + personaEmpleado.persona.ApMaterno;

                    var result = await _userManager.UpdateAsync(userIdentity);
                    if (!result.Succeeded)
                    {
                        throw new CustomException(result.Errors.First().Description);
                    }
                    if (isChangePwd)
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(userIdentity);
                        var resultPwd = await _userManager.ResetPasswordAsync(userIdentity, token,
                            pwdUnHashed);
                        if (!resultPwd.Succeeded)
                        {
                            throw new CustomException(result.Errors.First().Description);
                        }
                    }

                    _unitOfWork.SaveChangesAsync().Wait();
                    _logger.LogInformation("Se realiza la actualización de la persona con id " + persona.Id);
                    _logger.LogInformation("Se realiza la actualización del empleado con id " + empleado.Id);
                    ViewBag.Exito = Constantes.EXITO;
                    List<Roles> ListRoles = _unitOfWork.RolesRepository.getRoles();
                    return PartialView("Edit", new PersonaEmpleadoVO(ListRoles));
                }
                catch (FormatException e)
                {
                    _logger.LogWarning("Se presento error de tipo de dato: " + e.Message);
                    ViewData["error"] = e.Message;
                }
                catch (CustomException e)
                {
                    _logger.LogError("Se presento error en la actualización del empleado: " + e.Message);
                    ViewData["error"] = e.Message;
                }
                catch (Exception e)
                {
                    _logger.LogCritical("Se presento error en la actualización del empleado: " + e.Message);
                    ViewData["error"] = Messages.ERROR_MESSAGE;
                }
            }
            List<Roles> roles = _unitOfWork.RolesRepository.getRoles();
            personaEmpleado.roles = roles;
            return PartialView("Edit", personaEmpleado);
        }

        /// <summary>
        /// Método encargado del cambio de estatus de la persona y empleado
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
                Persona persona = await _unitOfWork.PersonasRepository.getPersonaEmpleadoById((long)id!);
                if (persona == null)
                {
                    throw new CustomException("El empleado no existe.");
                }
                Empleados empleado = await _unitOfWork.EmpleadosRepository.GetById(persona.Empleado.Id);
                if (empleado == null)
                {
                    throw new CustomException("El empleado no existe.");
                }
                string opcion = "re-activación";
                if (persona.EsActivo) opcion = "desactiva";
                persona.UsuarioModificacion = _user.Id.ToString();
                _unitOfWork.PersonasRepository.UpdateEstatus(persona);
                empleado.UsuarioModificacion = _user.Id.ToString();
                empleado.Persona = persona;
                _unitOfWork.EmpleadosRepository.UpdateEstatus(empleado);
                _unitOfWork.SaveChangesAsync().Wait();
                _logger.LogInformation("Se realiza cambio de estatus (" + opcion + ") a persona con id: " 
                    + persona.Id);
                _logger.LogInformation("Se realiza cambio de estatus (" + opcion + ") a empleado con : " 
                    + empleado.Id);
                resultado = Constantes.EXITO;
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error al cambiar estatus de persona (empleado) id " + id + ": " + e.Message);
                return Content(string.Format("{0}", resultado));
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error al cambiar estatus de persona (empleado) id " + id + ": "
                    + e.Message);
                return Content(string.Format("{0}", resultado));
            }
            return Content(string.Format("{0}", resultado));
        }

        /// <summary>
        /// Método encargado de la eliminación lógica de la persona y empleado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Int64? id)
        {
            int resultado = Constantes.ERROR;
            try
            {
                Persona persona = await _unitOfWork.PersonasRepository.getPersonaEmpleadoById((long)id!);
                if (persona == null)
                {
                    throw new CustomException("El empleado no existe.");
                }
                Empleados empleado = await _unitOfWork.EmpleadosRepository.GetById(persona.Empleado.Id);
                if (empleado == null)
                {
                    throw new CustomException("El empleado no existe.");
                }
                persona.UsuarioModificacion = _user.Id.ToString();
                _unitOfWork.PersonasRepository.Delete(persona);
                empleado.UsuarioModificacion = _user.Id.ToString();
                empleado.Persona = persona;
                _unitOfWork.EmpleadosRepository.Delete(empleado);
                _unitOfWork.SaveChangesAsync().Wait();
                _logger.LogInformation("Se da de baja a la persona con id " + persona.Id);
                _logger.LogInformation("Se da de baja al empleado con id " + empleado.Id);
                resultado = Constantes.EXITO;
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error al dar de baja a la persona (empleado) idPersona " + id);
                return Content(string.Format("{0}", resultado));
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error al dar de baja a la persona (empleado) idPersona " 
                    + id);
                return Content(string.Format("{0}", resultado));
            }
            return Content(string.Format("{0}", resultado));
        }

    }
}
