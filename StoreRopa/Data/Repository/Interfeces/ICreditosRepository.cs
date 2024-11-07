using StoreRopa.Models;

namespace StoreRopa.Data.Repository.Interfeces
{
    public interface ICreditosRepository: IRepository<Creditos>
    {
        public Creditos GetCreditoByDetalleVentaId(Int64 id);
    }
}
