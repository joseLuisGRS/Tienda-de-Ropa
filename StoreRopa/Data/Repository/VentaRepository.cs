using Microsoft.EntityFrameworkCore;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.utils;
using StoreRopa.Models;
using StoreRopa.Models.Vo;

namespace StoreRopa.Data.Repository
{
    public class VentaRepository: BaseRepository<Ventas>, IVentaRepository
    {
        public VentaRepository(StoreDBContext dBContext) : base(dBContext){ }

        public IEnumerable<Ventas> GetVentasCreditoById(Int64 id)
        {
            try
            {
                return _entities
                    .Include(e => e.DetalleVentas.Where(dv => dv.EsActivo == Constantes.ACTIVO))
                    .Where(e => e.EsVentaCredito == Constantes.ACTIVO &&
                        e.ClienteId == id &&
                        e.PendientePago > 0 &&
                        e.EsActivo == Constantes.ACTIVO)
                    .OrderBy(e => e.Id).AsNoTracking().AsEnumerable();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<DatosVentaAbonoVO> GetTotalesVentasById(long id)
        {
            try
            {
                if(id > 0)
                {
                    return new DatosVentaAbonoVO
                    {
                        Cantidad = _entities.Where(e => e.EsActivo == Constantes.ACTIVO 
                            && e.FechaAlta.Date == DateTime.Now.Date 
                            && e.EmpleadoId == id).Count(),
                        Importe = _entities.Where(e => e.EsActivo == Constantes.ACTIVO
                            && e.FechaAlta.Date == DateTime.Now.Date
                            && e.EmpleadoId == id).Sum(p => p.AbonoVenta)
                    };  
                }
                else
                {
                    return new DatosVentaAbonoVO
                    {
                        Cantidad = _entities.Where(e => e.EsActivo == Constantes.ACTIVO 
                            && e.FechaAlta.Date == DateTime.Now.Date).Count(),
                        Importe = _entities.Where(e => e.EsActivo == Constantes.ACTIVO 
                         && e.FechaAlta.Date == DateTime.Now.Date).Sum(p => p.AbonoVenta)
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
