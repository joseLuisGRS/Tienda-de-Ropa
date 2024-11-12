using StoreRopa.Models;
using StoreRopa.Models.Vo;

namespace StoreRopa.Data.Repository.Interfeces
{
    public interface IVentaRepository: IRepository<Ventas>
    {
        public IEnumerable<Ventas> GetVentasCreditoById(Int64 id);
        public Task<DatosVentaAbonoVO> GetTotalesVentasById(Int64 id);
    }
}
