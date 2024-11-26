using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreRopa.Models
{
    public class Devoluciones: BaseEntity
    {
        [Column(Order =2)]
        public Int64 DetalleVentaId { get; set; }
        public virtual DetalleVentas DetalleVentas { get; set; }
        [Column(Order =3)]
        public Int64 ClienteId { get; set;}
        public virtual Cliente Cliente { get; set; }
        [Column(Order =4, TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Importe { get; set; }
    }
}
