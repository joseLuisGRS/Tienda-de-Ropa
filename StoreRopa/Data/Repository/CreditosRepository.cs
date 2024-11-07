using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using System.Data.Entity;

namespace StoreRopa.Data.Repository
{
    public class CreditosRepository: BaseRepository<Creditos>, ICreditosRepository
    {
        public CreditosRepository(StoreDBContext dbContext) : base(dbContext) { }

        public Creditos GetCreditoByDetalleVentaId(Int64 id)
        {
            try
            {
                return _entities.AsNoTracking()
                    .FirstOrDefault(e => e.DetalleVentaId == id && e.EsActivo == Constantes.ACTIVO);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
