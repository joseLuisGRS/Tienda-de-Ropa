using StoreRopa.Models;
using StoreRopa.Models.utils;

namespace StoreRopa.Data.Repository.Interfeces
{
    public interface IVentaRepository: IRepository<Ventas>
    {
        public IEnumerable<Ventas> GetVentasCreditoById(Int64 id);
    }
}
