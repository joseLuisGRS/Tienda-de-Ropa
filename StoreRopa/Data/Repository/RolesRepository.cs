using Microsoft.EntityFrameworkCore;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.utils;

namespace StoreRopa.Data.Repository
{
    public class RolesRepository : BaseRepository<Roles>, IRolesRepository
    {

        public RolesRepository(StoreDBContext bdContext) : base(bdContext)
        {
        }

        /// <summary>
        /// Método encargado de la obtención de roles que no han sido eliminados logicamente
        /// recibe como parametros lel tamaño de la paginación y número de página a consultar
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<PagedResult<Roles>> GetAllRoles(int pageSize = 1, int page = 1)
        {
            try
            {
                return await _entities.AsNoTracking().Where(e => e.EsEliminado == Constantes.INACTIVO)
                    .OrderBy(e => e.Nombre).GetPagedResultAsync(pageSize, page);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// metodo para buscar un rol por nombre
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Roles> getRolByName(string name)
        {
            try
            {
                return await _entities.AsNoTracking().FirstOrDefaultAsync(p => p.Nombre.ToLower() == name.ToLower().Trim());
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        /// <summary>
        /// metodo para buscar los roles activos
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Roles> getRoles()
        {
            try
            {
                return _entities.AsNoTracking().Where(e => e.EsEliminado == Constantes.INACTIVO 
                    && e.EsActivo == Constantes.ACTIVO).OrderBy(e => e.Nombre).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
