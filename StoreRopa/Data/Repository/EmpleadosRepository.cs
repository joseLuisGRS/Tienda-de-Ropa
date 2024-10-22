using Microsoft.EntityFrameworkCore;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.utils;
using StoreRopa.Models.Vo;

namespace StoreRopa.Data.Repository
{
    public class EmpleadosRepository : BaseRepository<Empleados>, IEmpleadosRepository
    {
        private readonly CurrentUser _currentUser;
        private readonly User _user;
        public EmpleadosRepository(StoreDBContext bdContext, CurrentUser currentUser) : base(bdContext)
        {
            _currentUser = currentUser;
            _user = _currentUser.Builder();
        }

        /// <summary>
        /// Método encargado de la obtención de personas, empleados, roles que no han sido eliminados logicamente
        /// recibe como parametros lel tamaño de la paginación y número de página a consultar
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<PagedResult<Empleados>> GetEmpleados(int pageSize, int page)
        {
            try
            {
                string user = "";
                if (_user.UserName.ToLower() != Constantes.SUPER_ADMIN.ToLower()) user = Constantes.SUPER_ADMIN.ToLower();
                return await _entities.AsNoTracking().Include(e => e.Persona).Include(e => e.Rol)
                    .Where(e => e.EsEliminado == Constantes.INACTIVO && e.Usuario.ToLower() != user)
                    .OrderBy(e => e.Persona.Nombres)
                    .GetPagedResultAsync(pageSize, page);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// Método encargado de recuperar el empleado en base al usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Empleados> GetEmpleadoByUser(string user) {
            try
            {
                return await _entities.AsNoTracking().FirstOrDefaultAsync(e => e.Usuario.ToLower() == user.ToLower());
            }
            catch(Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Método encargado de recuperar el empleado en base al RolID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Empleados> GetEmpleadoRolByRolId(Int64? id) {
            try
            {
                return await _entities.AsNoTracking().Include(e => e.Rol).Where(e => e.EsActivo == Constantes.ACTIVO)
                    .FirstOrDefaultAsync(e => e.Rol.Id == id);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// Método para obtener persona Empleado y Rol by idPersona
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Empleados> getPersonaEmpleadoByIdPersona(Int64? id) {
            try
            {
                return await _entities.AsNoTracking().Include(e => e.Persona).Include(e => e.Rol)
                    .FirstOrDefaultAsync(e => e.PersonaId == id);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Empleados> getEmpleadoAndRolByUser(string user)
        {
            try
            {
                return await _entities.AsNoTracking().Include(e => e.Persona).Include(e => e.Rol)
                    .Where(e => e.EsActivo == Constantes.ACTIVO && e.Persona.EsActivo == Constantes.ACTIVO 
                        && e.Rol.EsActivo == Constantes.ACTIVO)
                    .FirstOrDefaultAsync(e => e.Usuario == user);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// Método para obtener persona Empleado by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Empleados> GetEmpleadoPersonaById(Int64? id)
        {
            try
            {
                return await _entities.AsNoTracking().Include(e => e.Persona).FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
