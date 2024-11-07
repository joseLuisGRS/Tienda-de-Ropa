using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Models;

namespace StoreRopa.Data.Repository
{
    public class AbonosRepository : BaseRepository<Abonos>, IAbonosRepository
    {
        public AbonosRepository(StoreDBContext bdContext) : base(bdContext)
        {
        }
    }
}
