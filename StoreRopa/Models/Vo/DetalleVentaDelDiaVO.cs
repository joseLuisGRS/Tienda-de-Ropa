namespace StoreRopa.Models.Vo
{
    public class DetalleVentaDelDiaVO
    {
        public string RolUsuario { get; set; }
        public int Ventas { get; set; }
        public decimal ImporteVenta { get; set; }
        public int Abonos { get; set; }
        public decimal ImporteAbonos { get; set; }
        public decimal ImporteTotal { get; set; }
        public int VentasG { get; set; }
        public decimal ImporteVentaG { get; set; }
        public int AbonosG { get; set; }
        public decimal ImporteAbonosG { get; set; }
        public decimal ImporteTotalG { get; set; }

    }
}
