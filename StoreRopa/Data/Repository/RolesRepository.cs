using Microsoft.EntityFrameworkCore;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.utils;
using StoreRopa.Models.Vo;

namespace StoreRopa.Data.Repository
{
    public class RolesRepository : BaseRepository<Roles>, IRolesRepository
    {
        private readonly CurrentUser _currentUser;
        private readonly User _user;
        public RolesRepository(StoreDBContext bdContext, CurrentUser currentUser) : base(bdContext)
        {
            _currentUser = currentUser;
            _user = _currentUser.Builder();
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
                string user = "";
                if (_user.UserName.ToLower() != Constantes.SUPER_ADMIN.ToLower()) user = Constantes.SUPER_ADMIN.ToLower();
                return await _entities.AsNoTracking().Where(e => e.EsEliminado == Constantes.INACTIVO && e.Nombre.ToLower() != user)
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
                string user = "";
                if (_user.UserName.ToLower() != Constantes.SUPER_ADMIN.ToLower()) user = Constantes.SUPER_ADMIN.ToLower();
                return _entities.AsNoTracking().Where(e => e.EsEliminado == Constantes.INACTIVO 
                    && e.EsActivo == Constantes.ACTIVO && e.Nombre != user).OrderBy(e => e.Nombre).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
