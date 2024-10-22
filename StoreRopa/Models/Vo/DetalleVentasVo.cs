namespace StoreRopa.Models.Vo
{
    public class DetalleVentasVo
    {
        public DetalleVentasVo(DetalleVentas detalleVentas) 
        { 
            DetalleVentas = detalleVentas;
        }
        public DetalleVentas DetalleVentas { get; set; }
        public decimal abonoArticulo { get; set; }
    }
}
