using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Models;

namespace StoreRopa.Data.Repository
{
    public class DetalleVentaRepository: BaseRepository<DetalleVentas>, IDetalleVentaRepository
    {
        public DetalleVentaRepository(StoreDBContext dBContext) : base(dBContext) { }
    }
}
