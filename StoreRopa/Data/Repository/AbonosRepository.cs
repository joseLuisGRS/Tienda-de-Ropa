using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.Vo;

namespace StoreRopa.Data.Repository
{
    public class AbonosRepository : BaseRepository<Abonos>, IAbonosRepository
    {
        public AbonosRepository(StoreDBContext bdContext) : base(bdContext)
        {
        }

        public async Task<DatosVentaAbonoVO> GetTotalesAbonosById(long id)
        {
            try
            {
                if (id > 0)
                {
                    return new DatosVentaAbonoVO
                    {
                        Cantidad = _entities.Where(e => e.EsActivo == Constantes.ACTIVO
                            && e.FechaAlta.Date == DateTime.Now.Date
                            && e.UsuarioAlta == id.ToString()).Count(),
                        Importe = _entities.Where(e => e.EsActivo == Constantes.ACTIVO
                            && e.FechaAlta.Date == DateTime.Now.Date
                            && e.UsuarioAlta == id.ToString()).Sum(p => p.Abono)
                    };
                }
                else
                {
                    return new DatosVentaAbonoVO
                    {
                        Cantidad = _entities.Where(e => e.EsActivo == Constantes.ACTIVO
                            && e.FechaAlta.Date == DateTime.Now.Date).Count(),
                        Importe = _entities.Where(e => e.EsActivo == Constantes.ACTIVO
                         && e.FechaAlta.Date == DateTime.Now.Date).Sum(p => p.Abono)
                    };
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
