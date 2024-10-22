using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Models;

namespace StoreRopa.Data.Repository
{
    public class CreditosRepository: BaseRepository<Creditos>, ICreditosRepository
    {
        public CreditosRepository(StoreDBContext dbContext) : base(dbContext) { }
    }
}
