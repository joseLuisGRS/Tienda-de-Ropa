using Microsoft.EntityFrameworkCore;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;

namespace StoreRopa.Data.Repository
{
    public class VentaRepository: BaseRepository<Ventas>, IVentaRepository
    {
        public VentaRepository(StoreDBContext dBContext) : base(dBContext){ }

        public IEnumerable<Ventas> GetVentasCreditoById(Int64 id)
        {
            try
            {
                return _entities
                    .Include(e => e.DetalleVentas.Where(dv => dv.EsActivo == Constantes.ACTIVO))
                    .Where(e => e.EsVentaCredito == Constantes.ACTIVO &&
                        e.ClienteId == id &&
                        e.PendientePago > 0 &&
                        e.EsActivo == Constantes.ACTIVO)
                    .OrderBy(e => e.Id).AsNoTracking().AsEnumerable();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
