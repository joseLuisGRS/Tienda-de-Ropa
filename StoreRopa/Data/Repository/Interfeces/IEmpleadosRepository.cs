using StoreRopa.Models;
using StoreRopa.Models.utils;

namespace StoreRopa.Data.Repository.Interfeces
{
    public interface IEmpleadosRepository : IRepository<Empleados>
    {
        public Task<PagedResult<Empleados>> GetEmpleados(int pageSize, int page);
        public Task<Empleados> GetEmpleadoByUser(string user);
        public Task<Empleados> GetEmpleadoRolByRolId(Int64? id);
        public Task<Empleados> getPersonaEmpleadoByIdPersona(Int64? id);

        public Task<Empleados> getEmpleadoAndRolByUser(string user);
    }
}
