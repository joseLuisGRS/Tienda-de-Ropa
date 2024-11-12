using StoreRopa.Models;
using StoreRopa.Models.Vo;

namespace StoreRopa.Data.Repository.Interfeces
{
    public interface IAbonosRepository: IRepository<Abonos>
    {
        public Task<DatosVentaAbonoVO> GetTotalesAbonosById(Int64 id);
    }
}
