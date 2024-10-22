using Microsoft.EntityFrameworkCore;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.utils;
using StoreRopa.Models.Vo;

namespace StoreRopa.Data.Repository
{
    public class PersonasRepository : BaseRepository<Persona>, IPersonasRepository
    {

        public PersonasRepository(StoreDBContext bdContext) : base(bdContext)
        {
        }

        /// <summary>
        /// Método encargado de la obtención de clientes que no han sido eliminados logicamente
        /// recibe como parametros lel tamaño de la paginación y número de página a consultar
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns>Clientes encontrados en BD</returns>
        public async Task<PagedResult<Persona>> GetClientes(int pageSize, int page)
        {
            try
            {
                return await _entities.AsNoTracking().Include(e => e.Cliente)
                    .Where(e => e.Cliente.EsEliminado == Constantes.INACTIVO).OrderBy(e => e.Nombres)
                    .GetPagedResultAsync(pageSize, page);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// metodo para buscar a a persona por curp
        /// </summary>
        /// <param name="curp"></param>
        /// <returns></returns>
        public Task<Persona> getPersonaByCurp(string curp)
        {
            try
            {
                return _entities.AsNoTracking().FirstOrDefaultAsync(p => p.Curp == curp);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Person and Cliente by id persona
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Persona> getPersonaClienteById(Int64? id)
        {
            try
            {
                return await _entities.AsNoTracking().Include(e => e.Cliente).FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// método para recuperar persaona y empleado por id persona
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Persona> getPersonaEmpleadoById(Int64? id) {
            try
            {
                return await _entities.AsNoTracking().Include(e => e.Empleado).FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Método encargado de la obtención de clientes que no han sido eliminados logicamente
        /// buscando por coincidencia ya sea por idCliente o curp
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="idCliente"></param>
        /// <param name="curp"></param>
        /// <returns>Clientes encontrados en BD</returns>
        public async Task<PagedResult<Persona>> GetClientesByCoincidencia(int pageSize, int page, Int64 idCliente, 
            string curp, int tipoConsulta)
        {
            try
            {
                if (tipoConsulta == 1)
                {
                    return await _entities.AsNoTracking().Include(e => e.Cliente)
                    .Where(e => e.Cliente.EsEliminado == Constantes.INACTIVO && e.Cliente.EsActivo == Constantes.ACTIVO
                        && e.Cliente.Id.ToString().Contains(idCliente.ToString())).OrderBy(e => e.Nombres)
                    .GetPagedResultAsync(pageSize, page);
                }
                else {
                    return await _entities.AsNoTracking().Include(e => e.Cliente)
                    .Where(e => e.Cliente.EsEliminado == Constantes.INACTIVO && e.Cliente.EsActivo == Constantes.ACTIVO
                        && e.Curp.Contains(curp)).OrderBy(e => e.Nombres)
                    .GetPagedResultAsync(pageSize, page);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Persona> Auth(AuthVo authVo) {
            try
            {
                return await _entities.AsNoTracking().Include(e => e.Empleado).FirstOrDefaultAsync(e => 
                    e.Empleado.Usuario == authVo.Usuario && e.Empleado.Password == authVo.Password);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
