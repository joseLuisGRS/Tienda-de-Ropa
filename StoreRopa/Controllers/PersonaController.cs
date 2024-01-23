using Microsoft.AspNetCore.Mvc;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.Vo;

namespace StoreRopa.Controllers
{
    public class PersonaController : Controller
    {
        private readonly ILogger<PersonaController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public PersonaController(ILogger<PersonaController> logger, IUnitOfWork unitOfWork) {
            this._logger = logger;
            this._unitOfWork = unitOfWork;
        }
        
        /// <summary>
        /// Método encargado de la obtención de clientes que no han sido eliminados logicamente
        /// recibe como parametros lel tamaño de la paginación y número de página a consultar
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns>Clientes encontrados en BD</returns>
        [HttpGet("/Clientes")]
        public async Task<IActionResult> GetClientes(int pageSize = 1, int page = 1)
        {
            try
            {
                if (pageSize == 1) pageSize = Constantes.PAGE_SIZE;
                _logger.LogInformation("Se realiza búsqueda de clientes.");              
                return View("Index", this._unitOfWork.PersonasRepository.GetClientes(pageSize, page).Result);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en la búsqueda de clientes: " + e.Message);
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return PartialView("Index", null);
            }
        }
        
        /// <summary>
        /// Método encargado de direccionar a la vista para el registro de clientes
        /// </summary>
        /// <returns>Vista para el registro de clientes</returns>
        public async Task<IActionResult> Create()
        {
                return PartialView("Create", new PersonaClienteVO());
        }
        
        /// <summary>
        /// Método para el registro de personas y clientes
        /// </summary>
        /// <param name="personaCliente"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonaClienteVO personaCliente)
        {
            ViewBag.Exito = Constantes.ERROR;
            ModelState.Remove("persona.Cliente");
            ModelState.Remove("cliente.Persona");
            ModelState.Remove("persona.Empleado");
            if (ModelState.IsValid)
            {
                try {
                    if (!Int64.TryParse(personaCliente.persona.Telefono, out Int64 tel)) {
                        throw new FormatException("Verifique el teléfono.");
                    }
                    Persona personCurp = await this._unitOfWork.PersonasRepository
                        .getPersonaByCurp(personaCliente.persona.Curp);
                    if (personCurp != null)
                    {
                        throw new CustomException("La curp ya se encuentra registrada.");
                    }
                    personaCliente.persona.UsuarioAlta = "prueba";
                    Persona persona = personaCliente.persona;
                    Cliente cliente = personaCliente.cliente;
                    this._unitOfWork.PersonasRepository.Create(persona);
                    cliente.UsuarioAlta = "prueba";
                    cliente.Persona = persona;
                    this._unitOfWork.ClientesRepository.Create(cliente);
                    this._unitOfWork.SaveChangesAsync().Wait();
                    _logger.LogInformation("Se realiza registro en BD de la persona con id " +  persona.Id);
                    _logger.LogInformation("Se realiza registro en BD del cliente con id " + cliente.Id);
                    ViewBag.Exito = Constantes.EXITO;
                    return PartialView("Create", new PersonaClienteVO());
                }
                catch (FormatException e) {
                    _logger.LogWarning("Se presento error de tipo de dato: " + e.Message);
                    ViewData["error"] = e.Message;
                }
                catch (CustomException e) {
                    _logger.LogError("Se presento error en el registro de clientes: " + e.Message);
                    ViewData["error"] = e.Message;
                }
                catch (AggregateException e) {
                    _logger.LogError("Se presento error en el registro de clientes: " + e.Message);
                    ViewData["error"] = Messages.ERROR_MESSAGE;
                }
                catch (Exception e) {
                    _logger.LogCritical("Se presento error en el registro de clientes: " + e.Message);
                    ViewData["error"] = Messages.ERROR_MESSAGE;
                }                
            }
            return PartialView("Create", new PersonaClienteVO());
        }
        
        /// <summary>
        /// Método para mostrar el detalle de la persona y cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Detail(Int64? id)
        {
            try
            {
                if (id == null) throw new CustomException("Cliente no encontrado.");
                var persona = await this._unitOfWork.PersonasRepository.getPersonaClienteById(id);
                if (persona == null) throw new CustomException("Cliente no encontrado.");
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
                _logger.LogCritical("Se presento un error al buscar persona (cliente) con id " + id + ": " +e.Message);
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return PartialView("Detail", null);
            }
        }
        
        /// <summary>
        /// Método que carga la vista con los datos de persona y cliente que se modificarán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Int64? id)
        {
            try
            {
                if (id == null) throw new CustomException("Cliente no encontrado.");
                var persona = await this._unitOfWork.PersonasRepository.getPersonaClienteById(id);
                if (persona == null) throw new CustomException("Cliente no encontrado.");
                return PartialView("Edit", new PersonaClienteVO(persona, persona.Cliente));
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error en la edición del cliente idPersona " + id + ": " + e.Message);
                ViewData["error"] = e.Message;
                return PartialView("Edit", null);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en la edición del cliente idPersona " + id + ": " + e.Message);
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return PartialView("Edit", null);
            }
        }
        
        /// <summary>
        /// Método encargado de la actualización del cliente y persona
        /// </summary>
        /// <param name="personaCliente"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PersonaClienteVO personaCliente)
        {
            ViewBag.Exito = Constantes.ERROR;
            ModelState.Remove("persona.Cliente.Persona");
            ModelState.Remove("cliente");
            ModelState.Remove("persona.Empleado");
            if (ModelState.IsValid)
            {
                try
                {
                    Persona person = await this._unitOfWork.PersonasRepository.GetById(personaCliente.persona.Id);
                    if (person == null)
                    {
                        throw new CustomException("El cliente no existe.");
                    }
                    Cliente client = await this._unitOfWork.ClientesRepository.GetById(personaCliente.persona.Cliente.Id);
                    if (client == null)
                    {
                        throw new CustomException("El cliente no existe.");
                    }
                    if (!Int64.TryParse(personaCliente.persona.Telefono, out Int64 tel))
                    {
                        throw new FormatException("Verifique el teléfono.");
                    }
                    Persona personCurp = await this._unitOfWork.PersonasRepository
                        .getPersonaByCurp(personaCliente.persona.Curp);
                    if (personCurp != null && personCurp.Id != personaCliente.persona.Id)
                    {
                        throw new CustomException("La curp ya se encuentra registrada.");
                    }
                    personaCliente.persona.UsuarioModificacion = "prueba";
                    personaCliente.persona.FechaAlta = person.FechaAlta;
                    personaCliente.persona.UsuarioAlta = person.UsuarioAlta;
                    personaCliente.persona.EsActivo = person.EsActivo;
                    Persona persona = personaCliente.persona;
                    Cliente cliente = personaCliente.persona.Cliente;
                    this._unitOfWork.PersonasRepository.Update(persona);
                    cliente.UsuarioModificacion = "prueba";
                    cliente.FechaAlta = client.FechaAlta;
                    cliente.Saldo = client.Saldo;
                    cliente.UsuarioAlta = client.UsuarioAlta;
                    cliente.EsActivo = client.EsActivo;
                    cliente.Persona = persona;
                    cliente.Id = client.Id;
                    this._unitOfWork.ClientesRepository.Update(cliente);
                    this._unitOfWork.SaveChangesAsync().Wait();
                    _logger.LogInformation("Se realiza la actualización de la persona con id: " + persona.Id);
                    _logger.LogInformation("Se realiza la actualización del cliente con id: " + cliente.Id);
                    ViewBag.Exito = Constantes.EXITO;
                    return PartialView("Edit", new PersonaClienteVO());
                }
                catch (FormatException e)
                {
                    _logger.LogWarning("Se presento un error en el tipo de dato: " + e.Message);
                    ViewData["error"] = e.Message;
                }
                catch (CustomException e)
                {
                    _logger.LogError("Se presento un error en la edición del cliente: " + e.Message);
                    ViewData["error"] = e.Message;
                }
                catch (Exception e)
                {
                    _logger.LogCritical("Se presento un error en la edición del cliente: " + e.Message);
                    ViewData["error"] = Messages.ERROR_MESSAGE;
                }
            }
            return PartialView("Edit", new PersonaClienteVO());
        }

        /// <summary>
        /// Método encargado del cambio de estatus de la persona y cliente
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
                Persona persona = await this._unitOfWork.PersonasRepository.getPersonaClienteById((long)id);
                if (persona == null)
                {
                    throw new CustomException("El cliente no existe.");
                }
                Cliente cliente = await this._unitOfWork.ClientesRepository.GetById(persona.Cliente.Id);
                if (cliente == null)
                {
                    throw new CustomException("El cliente no existe.");
                }
                if (cliente.Saldo > 0 && persona.EsActivo)
                {
                    return Content(string.Format("{0}", 2));
                }
                string opcion = "re-activación";
                if (persona.EsActivo)  opcion = "desactiva";              
                persona.UsuarioModificacion = "prueba";
                this._unitOfWork.PersonasRepository.UpdateEstatus(persona);
                cliente.UsuarioModificacion = "prueba";
                cliente.Persona = persona;
                this._unitOfWork.ClientesRepository.UpdateEstatus(cliente);
                this._unitOfWork.SaveChangesAsync().Wait();
                _logger.LogInformation("Se realiza cambio de estatus (" + opcion + ") a persona con id: " + persona.Id);
                _logger.LogInformation("Se realiza cambio de estatus (" + opcion + ") a cliente con : " + cliente.Id);
                resultado =  Constantes.EXITO;
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error al cambiar estatus de persona (cliente) id " + id + ": "+ e.Message);
                return Content(string.Format("{0}", resultado));
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error al cambiar estatus de persona (cliente) id " + id + ": " 
                    + e.Message);
                return Content(string.Format("{0}", resultado));
            }
            return Content(string.Format("{0}", resultado));
        }

        /// <summary>
        /// Método encargado de la eliminación lógica de la persona y cliente
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
                Persona persona = await this._unitOfWork.PersonasRepository.getPersonaClienteById((long)id);
                if (persona == null)
                {
                    throw new CustomException("El cliente no existe.");
                }
                Cliente cliente = await this._unitOfWork.ClientesRepository.GetById(persona.Cliente.Id);
                if (cliente == null)
                {
                    throw new CustomException("El cliente no existe.");
                }
                if (cliente.Saldo > 0) {
                    return Content(string.Format("{0}", 2));
                }
                persona.UsuarioModificacion = "prueba";
                this._unitOfWork.PersonasRepository.Delete(persona);
                cliente.UsuarioModificacion = "prueba";
                cliente.Persona = persona;
                this._unitOfWork.ClientesRepository.Delete(cliente);
                this._unitOfWork.SaveChangesAsync().Wait();
                _logger.LogInformation("Se da de baja a la persona con id " + persona.Id);
                _logger.LogInformation("Se da de baja al cliente con id " + cliente.Id);
                resultado = Constantes.EXITO;
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error al dar de baja a la persona (cliente) idPersona " + id);
                return Content(string.Format("{0}", resultado));
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error al dar de baja a la persona (cliente) idPersona " + id);
                return Content(string.Format("{0}", resultado));
            }
            return Content(string.Format("{0}" , resultado));
        }

    }
}