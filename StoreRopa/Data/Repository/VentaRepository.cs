using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Models;

namespace StoreRopa.Data.Repository
{
    public class VentaRepository: BaseRepository<Ventas>, IVentaRepository
    {
        public VentaRepository(StoreDBContext dBContext) : base(dBContext){ }
    }
}
