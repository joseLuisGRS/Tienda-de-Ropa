namespace StoreRopa.Models.Vo
{
    public class TicketAbonoVO
    {
        public DateTime FechaAbono { get; set; }
        public string UsuarioAlta {  get; set; }
        public Int64 ClienteId { get; set;}
        public List<DetalleAbonoVO> DetalleAbonos { get; set; }
        public decimal TotalAbono { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal Efectivo { get; set; }
        

    }
}
