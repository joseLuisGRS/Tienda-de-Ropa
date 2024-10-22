using Microsoft.EntityFrameworkCore;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;

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

        public async Task<Cliente> GetClientePersonaById(Int64 id)
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
