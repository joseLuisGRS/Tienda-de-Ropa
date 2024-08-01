using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.Vo;

namespace StoreRopa.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly CurrentUser _currentUser;
        private readonly IEmpleadosRepository _empleadosRepository;
        public AuthController(ILogger<AuthController> logger, IUnitOfWork unitOfWork, 
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, CurrentUser currentUser,
            IEmpleadosRepository empleadosRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _currentUser = currentUser;
            _empleadosRepository = empleadosRepository;
        }

        /// <summary>
        /// Metodo que muestra la pantalla para el login
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                
                return View("Index");
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en el logín: " + e.Message);
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return View("Index");
            }
        }

        /// <summary>
        /// Metodo que valida los datos del usuario para darle el accesoo denegarlo,
        /// LLena la clase CurrentUser, con los datos de la sesión
        /// </summary>
        /// <param name="authVo"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Auth(AuthVo authVo)
        {
            if (ModelState.IsValid)
            {                
                var result = await _signInManager.PasswordSignInAsync(authVo.Usuario, authVo.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    Empleados empleado = await _empleadosRepository.getEmpleadoAndRolByUser(authVo.Usuario);
                    if (empleado != null)
                    {
                        _currentUser
                            .IdB(Int32.Parse(empleado.Id.ToString()))
                            .FullNameB(empleado.Persona.Nombres + " " + empleado.Persona.ApPaterno + " " + empleado.Persona.ApMaterno)
                            .UserNameB(authVo.Usuario)
                            .RolNameB(empleado.Rol.Nombre)
                            .RolIdB(Int32.Parse(empleado.Rol.Id.ToString()))
                            .Builder();
                        return RedirectToAction("Index", "Home");
                    }
                }

                ViewData["errorSesion"] = "Usuario y/o contraseña incorrectos.!";
            }            
            return PartialView("Index", authVo);
                        
        }

        /// <summary>
        /// Metodo para salir de sesión y liberar recursos de usuario
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");

        }

        /// <summary>
        /// Metodo que muesta la pagina cuando no se tienen los permisos correspondientes
        /// </summary>
        /// <returns></returns>
        [HttpGet("Denied")]
        public IActionResult Denied()
        {
            try
            {

                return View("AccessDenied");
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error de permisos: " + e.Message);
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return View("AccessDenied");
            }
        }

        /// <summary>
        /// Metodo que solo se ejecuta una vez para realizar el registro del usuario y rol de SuperAdmin
        /// </summary>
        /// <returns></returns>
        [HttpGet("SuperAdmin")]
        public async Task<IActionResult> SuperAdmin()
        {
            try
            {
                string rolName = "SuperAdmin";
                string curp = "SUAU000101ABCDEF01";
                string userName = "SuperAdmin";
                string pwdUnHashed = "So9Ar1*c";

                //Add new role
                Roles rolBd = await _unitOfWork.RolesRepository.getRolByName(rolName);
                if (rolBd != null)
                {
                    throw new CustomException("El SuperAdmin ya se encuentra registrado.");
                }
                var rol = new Roles 
                {
                    Nombre= rolName, 
                    Descripcion = "Rol con todos los privilegios para la administración del sistema.",  
                    EsActivo = Constantes.ACTIVO,
                    EsEliminado = Constantes.INACTIVO,
                    FechaAlta = DateTime.UtcNow,
                    UsuarioAlta = "1"
                };
                await _unitOfWork.RolesRepository.Create(rol);

                //Add new Person
                Persona personCurp = await _unitOfWork.PersonasRepository.getPersonaByCurp(curp);
                if (personCurp != null)
                {
                    throw new CustomException("El SuperAdmin ya se encuentra registrado.");
                }
                Empleados empleadoBD = await _unitOfWork.EmpleadosRepository.GetEmpleadoByUser(userName);
                if (empleadoBD != null)
                {
                    throw new CustomException("El SuperAdmin ya se encuentra registrado.");
                }
                var pwdEncrypt = Encrypt.GetSHA256(pwdUnHashed);

                Persona persona = new Persona()
                {
                    Nombres = "Super",
                    ApPaterno = "Usuario",
                    ApMaterno = "Administrador",
                    Curp = curp,
                    FechaNacimiento = new DateTime(2000, 01, 01),
                    Municipio = "Villa Victoria",
                    Estado = "México",
                    Direccion = "Villa Victoria centro",
                    Numero = "SN",
                    Cp = 50960,
                    Telefono = "7224567890",
                    EsActivo = Constantes.ACTIVO,
                    EsEliminado = Constantes.INACTIVO,
                    FechaAlta = DateTime.UtcNow,
                    UsuarioAlta = "1"
                };                
                 await _unitOfWork.PersonasRepository.Create(persona);

                //Add new Employee
                Empleados empleado = new Empleados()
                {
                    PersonaId = persona.Id,
                    Persona = persona,
                    RolId = rol.Id,
                    Rol = rol,
                    Usuario = userName,
                    Password = pwdEncrypt,
                    EsActivo = Constantes.ACTIVO,
                    EsEliminado = Constantes.INACTIVO,
                    FechaAlta = DateTime.UtcNow,
                    UsuarioAlta = "1"                    
                };
                await _unitOfWork.EmpleadosRepository.Create(empleado);
                
                var fullName = persona.Nombres + " " + persona.ApPaterno + " " + persona.ApMaterno;
                var userIdentity = new ApplicationUser { UserName = empleado.Usuario, Email = "", FullName = fullName };
                var result = await _userManager.CreateAsync(userIdentity, pwdUnHashed);
                if (!result.Succeeded)
                {
                    throw new CustomException(result.Errors.First().Description);
                }
                _unitOfWork.SaveChangesAsync().Wait();
                _logger.LogInformation("Se realiza registro en BD del rol con id " + rol.Id);
                _logger.LogInformation("Se realiza registro en BD de la persona con id " + persona.Id);
                _logger.LogInformation("Se realiza registro en BD del empleado con id " + empleado.Id);

                ViewData["errorSesion"] = "Registro exitoso de SuperAdmin";
                return PartialView("Index");
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento error en el registro de SuperAdmin: " + e.Message);
                ViewData["errorSesion"] = e.Message;
                return PartialView("Index");
            }
            catch (AggregateException e)
            {
                _logger.LogError("Se presento error en el registro de SuperAdmin: " + e.Message);
                ViewData["errorSesion"] = Messages.ERROR_MESSAGE;
                return PartialView("Index");
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error al crear superAdmin: " + e.Message);
                ViewData["errorSesion"] = Messages.ERROR_MESSAGE;
                return PartialView("Index");
            }
        }

    }
}
