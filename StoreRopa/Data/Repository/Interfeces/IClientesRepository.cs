using StoreRopa.Models;

namespace StoreRopa.Data.Repository.Interfeces
{
    public interface IClientesRepository : IRepository<Cliente>
    {
        public IEnumerable<Cliente> GetClientesPersona();
    }
}
