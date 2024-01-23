using StoreRopa.Models;
using StoreRopa.Models.utils;

namespace StoreRopa.Data.Repository.Interfeces
{
    public interface IPersonasRepository : IRepository<Persona>
    {
        public Task<PagedResult<Persona>> GetClientes(int pageSize, int page);
        public Task<Persona> getPersonaByCurp(string curp);
        public Task<Persona> getPersonaClienteById(Int64? id);
        public Task<Persona> getPersonaEmpleadoById(Int64? id);
    }
}
