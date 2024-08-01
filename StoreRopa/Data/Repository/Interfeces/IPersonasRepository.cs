using StoreRopa.Models;
using StoreRopa.Models.utils;
using StoreRopa.Models.Vo;

namespace StoreRopa.Data.Repository.Interfeces
{
    public interface IPersonasRepository : IRepository<Persona>
    {
        public Task<PagedResult<Persona>> GetClientes(int pageSize, int page);
        public Task<Persona> getPersonaByCurp(string curp);
        public Task<Persona> getPersonaClienteById(Int64? id);
        public Task<Persona> getPersonaEmpleadoById(Int64? id);
        public Task<PagedResult<Persona>> GetClientesByCoincidencia(int pageSize, int page, Int64 idCliente, 
            string curp, int tipoConsulta);
        public Task<Persona> Auth(AuthVo authVo);
    }
}
