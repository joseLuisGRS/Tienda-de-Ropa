using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using StoreRopa.Data;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.utils;
using StoreRopa.Models.Vo;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace StoreRopa.Controllers
{
    public class PersonaController : Controller
    {
        private readonly ILogger<PersonaController> _logger;
        private readonly StoreDBContext _dBContext;
        public PersonaController(StoreDBContext dBContext, ILogger<PersonaController> logger) {
            this._dBContext= dBContext;
            this._logger = logger;
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
                var per = await _dBContext.Persona.AsNoTracking().Include(e => e.Cliente)
                    .Where(e => e.Cliente.EsEliminado == Constantes.INACTIVO).OrderBy(e => e.Nombres)
                    .GetPagedResultAsync(pageSize,page);
                return View("Index", per);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en la búsqueda de clientes: " + e.Message);
                _dBContext.Dispose();
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
            if (ModelState.IsValid)
            {
                using var transaction = _dBContext.Database.BeginTransaction();
                try {
                    if (!Int64.TryParse(personaCliente.persona.Telefono, out Int64 tel)) {
                        throw new FormatException("Verifique el teléfono.");
                    }
                    Persona personCurp = getPersonaByCurp(personaCliente.persona.Curp);
                    if (personCurp != null)
                    {
                        throw new CustomException("La curp ya se encuentra registrada.");
                    }
                    personaCliente.persona.UsuarioAlta = "prueba";
                    personaCliente.persona.EsActivo = Constantes.ACTIVO;
                    personaCliente.persona.FechaAlta = DateTime.Now;
                    Persona persona = personaCliente.persona;
                    Cliente cliente = personaCliente.cliente;       
                    _dBContext.Add(persona);
                    cliente.UsuarioAlta = "prueba";
                    cliente.EsActivo = Constantes.ACTIVO;
                    cliente.FechaAlta = DateTime.Now;
                    cliente.Persona = persona;
                    _dBContext.Add(cliente);
                    _dBContext.SaveChangesAsync().Wait();
                    _logger.LogInformation("Se realiza registro en BD de la persona con id " +  persona.Id);
                    _logger.LogInformation("Se realiza registro en BD del cliente con id " + cliente.Id);
                    ViewBag.Exito = Constantes.EXITO;
                    transaction.Commit();
                    _dBContext.Dispose();
                    return PartialView("Create", new PersonaClienteVO());
                }
                catch (FormatException e) {
                    _logger.LogWarning("Se presento error de tipo de dato: " + e.Message);
                    transaction.Rollback();
                    _dBContext.Dispose();
                    ViewData["error"] = e.Message;
                }
                catch (CustomException e) {
                    _logger.LogError("Se presento error en el registro de clientes: " + e.Message);
                    transaction.Rollback();
                    _dBContext.Dispose();
                    ViewData["error"] = e.Message;
                }
                catch (AggregateException e) {
                    _logger.LogError("Se presento error en el registro de clientes: " + e.Message);
                    transaction.Rollback();
                    _dBContext.Dispose();
                    ViewData["error"] = Messages.ERROR_MESSAGE;
                }
                catch (Exception e) {
                    _logger.LogCritical("Se presento error en el registro de clientes: " + e.Message);
                    transaction.Rollback();
                    _dBContext.Dispose();
                    ViewData["error"] = Messages.ERROR_MESSAGE;
                }                
            }
            return PartialView("Create", new PersonaClienteVO());
        }

        /// <summary>
        /// metodo para buscar a a persona por curp
        /// </summary>
        /// <param name="curp"></param>
        /// <returns></returns>
        public Persona getPersonaByCurp(string curp) {
            try {
                _logger.LogInformation("Se realiza consulta de persona en base a la curp: " +  curp);
                return _dBContext.Persona.AsNoTracking().FirstOrDefault(p => p.Curp == curp);
            }            
            catch (Exception e)
            {
                _logger.LogCritical("Se presento error en la consulta de persona con curp " + curp + ": " 
                    + e.Message);
                _dBContext.Dispose();
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return null;
            }
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
                var persona = await getPersonaById(id);
                if (persona == null) throw new CustomException("Cliente no encontrado.");
                return PartialView("Detail", persona);
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error al buscar persona (cliente) con id " + id + ": " 
                    + e.Message);
                _dBContext.Dispose();
                ViewData["error"] = e.Message;
                return PartialView("Detail", null);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error al buscar persona (cliente) con id " + id + ": " 
                    +e.Message);
                _dBContext.Dispose();
                ViewData["error"] = Messages.ERROR_MESSAGE;
                return PartialView("Detail", null);
            }
        }
        
        /// <summary>
        /// Get Person and Cliente by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Persona> getPersonaById(Int64? id)
        {
            try
            {
                _logger.LogInformation("Se realiza búsqueda de persona (cliente) con id " + id);
                return await _dBContext.Persona.AsNoTracking().Include(e => e.Cliente)
                   .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en la búsqueda de persona (cliente) con id " + id 
                    + ": " + e.Message);
                _dBContext.Dispose();
                return null;
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
                var persona = await getPersonaById(id);
                if (persona == null) throw new CustomException("Cliente no encontrado.");
                return PartialView("Edit", new PersonaClienteVO(persona, persona.Cliente));
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error en la edición del cliente idPersona " + id + ": " 
                    + e.Message);
                _dBContext.Dispose();
                ViewData["error"] = e.Message;
                return PartialView("Edit", null);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en la edición del cliente idPersona " + id + ": " 
                    + e.Message);
                _dBContext.Dispose();
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
            if (ModelState.IsValid)
            {
                using var transaction = _dBContext.Database.BeginTransaction();
                try
                {
                    Persona person = await getPersonaById(personaCliente.persona.Id);
                    if (person == null)
                    {
                        throw new CustomException("El cliente no existe.");
                    }
                    Cliente client = await getClienteById(personaCliente.persona.Cliente.Id);
                    if (client == null)
                    {
                        throw new CustomException("El cliente no existe.");
                    }
                    if (!Int64.TryParse(personaCliente.persona.Telefono, out Int64 tel))
                    {
                        throw new FormatException("Verifique el teléfono.");
                    }
                    Persona personCurp = getPersonaByCurp(personaCliente.persona.Curp);
                    if (personCurp != null && personCurp.Id != personaCliente.persona.Id)
                    {
                        throw new CustomException("La curp ya se encuentra registrada.");
                    }
                    personaCliente.persona.UsuarioModificacion = "prueba";
                    personaCliente.persona.FechaModificacion = DateTime.Now;
                    personaCliente.persona.FechaAlta = person.FechaAlta;
                    personaCliente.persona.UsuarioAlta = person.UsuarioAlta;
                    personaCliente.persona.EsActivo = person.EsActivo;
                    Persona persona = personaCliente.persona;
                    Cliente cliente = personaCliente.persona.Cliente;
                    _dBContext.Entry(persona).State = EntityState.Modified;
                    cliente.UsuarioModificacion = "prueba";
                    cliente.FechaModificacion = DateTime.Now;
                    cliente.FechaAlta = client.FechaAlta;
                    cliente.Saldo = client.Saldo;
                    cliente.UsuarioAlta = client.UsuarioAlta;
                    cliente.EsActivo = client.EsActivo;
                    cliente.Persona = persona;
                    _dBContext.Entry(cliente).State = EntityState.Modified;
                    _dBContext.SaveChangesAsync().Wait();
                    _logger.LogInformation("Se realiza la actualización de la persona con id: " + persona.Id);
                    _logger.LogInformation("Se realiza la actualización del cliente con id: " + cliente.Id);
                    ViewBag.Exito = Constantes.EXITO;
                    transaction.Commit();
                    _dBContext.DisposeAsync();
                    transaction.Dispose();
                    return PartialView("Edit", new PersonaClienteVO());
                }
                catch (FormatException e)
                {
                    _logger.LogWarning("Se presento un error en el tipo de dato: " + e.Message);
                    transaction.Rollback();
                    transaction.Dispose();
                    _dBContext.Dispose();
                    ViewData["error"] = e.Message;
                }
                catch (CustomException e)
                {
                    _logger.LogError("Se prsento un error en la edición del cliente: " + e.Message);
                    transaction.Rollback();
                    transaction.Dispose();
                    _dBContext.Dispose();
                    ViewData["error"] = e.Message;
                }
                catch (Exception e)
                {
                    _logger.LogCritical("Se presento un error en la edición del cliente: " + e.Message);
                    transaction.Rollback();
                    transaction.Dispose();
                    _dBContext.Dispose();
                    ViewData["error"] = Messages.ERROR_MESSAGE;
                }
            }
            return PartialView("Edit", new PersonaClienteVO());
        }

        /// <summary>
        /// Recupera el cliente 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Cliente> getClienteById(Int64? id)
        {
            try
            {
                _logger.LogInformation("Se realiza la búsqueda del cliente con id " + id);
                return await _dBContext.Cliente.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error en la búsqueda cliente con id " + id + ": " 
                    + e.Message);
                _dBContext.Dispose();
                return null;
            }
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
            using var transaction = _dBContext.Database.BeginTransaction();
            try
            {
                Persona persona = await getPersonaById(id);
                if (persona == null)
                {
                    throw new CustomException("El cliente no existe.");
                }
                Cliente cliente = await getClienteById(persona.Cliente.Id);
                if (cliente == null)
                {
                    throw new CustomException("El cliente no existe.");
                }
                if (cliente.Saldo > 0 && persona.EsActivo)
                {
                    return Content(string.Format("{0}", 2));
                }
                string opcion = "re-activación";
                if (persona.EsActivo)
                {
                    persona.EsActivo = false;
                    cliente.EsActivo = false;
                    opcion = "desactiva";
                }
                else {
                    persona.EsActivo = true;
                    cliente.EsActivo = true;
                }
                persona.UsuarioModificacion = "prueba";
                persona.FechaModificacion = DateTime.Now;
                _dBContext.Entry(persona).State = EntityState.Modified;
                cliente.UsuarioModificacion = "prueba";
                cliente.FechaModificacion = DateTime.Now;
                cliente.Persona = persona;
                _dBContext.Entry(cliente).State = EntityState.Modified;
                _dBContext.SaveChangesAsync().Wait();
                _logger.LogInformation("Se realiza cambio de estatus (" + opcion + ") a persona con id: " 
                    + persona.Id);
                _logger.LogInformation("Se realiza cambio de estatus (" + opcion + ") a cliente con : " 
                    + cliente.Id);
                transaction.Commit();
                _dBContext.DisposeAsync();
                transaction.Dispose();
                resultado =  Constantes.EXITO;
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error al cambiar estatus de persona (cliente) id " + id 
                    + ": "+ e.Message);
                transaction.Rollback();
                transaction.Dispose();
                _dBContext.Dispose();
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error al cambiar estatus de persona (cliente) id " + id 
                    + ": " + e.Message);
                transaction.Rollback();
                transaction.Dispose();
                _dBContext.Dispose();
            }
            return Content(string.Format("{0}", Constantes.EXITO));
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
            using var transaction = _dBContext.Database.BeginTransaction();
            try
            {
                Persona persona = await getPersonaById(id);
                if (persona == null)
                {
                    throw new CustomException("El cliente no existe.");
                }
                Cliente cliente = await getClienteById(persona.Cliente.Id);
                if (cliente == null)
                {
                    throw new CustomException("El cliente no existe.");
                }
                if (cliente.Saldo > 0) {
                    return Content(string.Format("{0}", 2));
                }
                persona.EsActivo = false;
                cliente.EsActivo = false;
                persona.EsEliminado = true;
                cliente.EsEliminado = true;                
                persona.UsuarioModificacion = "prueba";
                persona.FechaModificacion = DateTime.Now;
                _dBContext.Entry(persona).State = EntityState.Modified;
                cliente.UsuarioModificacion = "prueba";
                cliente.FechaModificacion = DateTime.Now;
                cliente.Persona = persona;
                _dBContext.Entry(cliente).State = EntityState.Modified;
                _dBContext.SaveChangesAsync().Wait();
                _logger.LogInformation("Se da de baja a la persona con id " + persona.Id);
                _logger.LogInformation("Se da de baja al cliente con id " + cliente.Id);
                transaction.Commit();
                _dBContext.DisposeAsync();
                transaction.Dispose();
            }
            catch (CustomException e)
            {
                _logger.LogError("Se presento un error al dar de baja a la persona (cliente) idPersona " + id);
                transaction.Rollback();
                transaction.Dispose();
                _dBContext.Dispose();
            }
            catch (Exception e)
            {
                _logger.LogCritical("Se presento un error al dar de baja a la persona (cliente) idPersona " + id);
                transaction.Rollback();
                transaction.Dispose();
                _dBContext.Dispose();
            }
            return Content(string.Format("{0}" , Constantes.EXITO));
        }
    }
}