namespace StoreRopa.Models.Vo
{
    public class GeneraVentaVO
    {
        public Ventas Venta { get; set; } = null!;
        public List<DetalleVentas>? DetallesDeVentas { get; set; }
    }
}
