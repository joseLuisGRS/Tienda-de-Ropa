using Microsoft.EntityFrameworkCore;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.utils;

namespace StoreRopa.Data.Repository
{
    public class ClientesRepository : BaseRepository<Cliente>, IClientesRepository
    {
        public ClientesRepository(StoreDBContext bdContext) : base(bdContext)
        {
        }

        public IEnumerable<Cliente> GetClientesPersona()
        {
            try
            {
                return _entities.AsNoTracking().Include(e => e.Persona).Where(e => e.EsActivo == Constantes.ACTIVO)
                    .OrderBy(e => e.Persona.Nombres).AsEnumerable();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
