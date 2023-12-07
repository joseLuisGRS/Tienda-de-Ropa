using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using StoreRopa.Data;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.Vo;
using System.Data.SqlTypes;

namespace StoreRopa.Controllers
{
    public class PersonaController : Controller
    {
        private readonly StoreDBContext _dBContext;
        public PersonaController(StoreDBContext dBContext) {
            this._dBContext= dBContext;
        }
        [HttpGet("/Clientes")]
        public async Task<IActionResult> GetClientes()
        {
            return View("Index", await _dBContext.Persona.AsNoTracking()
                .Include(e => e.Cliente).ToListAsync());
        }

        public async Task<IActionResult> Edit(Int64? id)
        {
            if (id == null) return NotFound();
            var persona = await _dBContext.Persona.FindAsync(id);
            if (persona == null) return NotFound();
            return View(persona);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Int64 id, [Bind("Id, Nombres, ApPaterno, ApMaterno, Curp, " +
            "FechaNacimiento, Ciudad, Pais, Direccion, Numero, Cp, Telefono, EsActivo, EsEliminado, " +
            "FechaAlta, UsuarioAlta, FechaModificacion, UsuarioModificacion")] Persona persona ) 
        {
            if (id != persona.Id) return NotFound();
            if (ModelState.IsValid) 
            { 
                _dBContext.Update(persona);
                _dBContext.SaveChangesAsync().Wait();
                return RedirectToAction(nameof(Index));
            }
            return View(persona); 
        }

        public async Task<IActionResult> Create()
        {
            PersonaCliente personaCliente = new PersonaCliente();
            return PartialView("Create", personaCliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int64 id, PersonaCliente personaCliente)
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
                    personaCliente.persona.FechaAlta = DateTime.Now;
                    personaCliente.persona.EsActivo = Constantes.ACTIVO;
                    personaCliente.persona.UsuarioAlta = "prueba";
                    Persona persona = personaCliente.persona;
                    Cliente cliente = personaCliente.cliente;       
                    _dBContext.Add(persona);
                    cliente.EsActivo = Constantes.ACTIVO;
                    cliente.Persona = persona;
                    _dBContext.Add(cliente);
                    _dBContext.SaveChangesAsync().Wait();
                    personaCliente = new PersonaCliente();
                    ViewBag.Exito = Constantes.EXITO;
                    transaction.Commit();
                    _dBContext.Dispose();
                    return PartialView("Create", personaCliente);
                }
                catch (FormatException e) {
                    transaction.Rollback();
                    _dBContext.Dispose();
                    ViewData["error"] = e.Message;
                }
                catch (CustomException e) {
                    transaction.Rollback();
                    _dBContext.Dispose();
                    ViewData["error"] = e.Message;
                }
                catch (AggregateException e) {
                    transaction.Rollback();
                    _dBContext.Dispose();
                    ViewData["error"] = Messages.ERROR_MESSAGE;
                }
                catch (Exception e) {
                    transaction.Rollback();
                    _dBContext.Dispose();
                    ViewData["error"] = Messages.ERROR_MESSAGE;
                }                
            }
            return PartialView("Create", personaCliente);
        }
        /// <summary>
        /// metodo para buscar a a persona por curp
        /// </summary>
        /// <param name="curp"></param>
        /// <returns></returns>
        public Persona getPersonaByCurp(string curp) {
            return _dBContext.Persona.AsNoTracking()
                    .FirstOrDefault(p => p.Curp == curp);
        }
    }
}
