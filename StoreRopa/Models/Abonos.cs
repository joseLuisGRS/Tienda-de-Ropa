using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoreRopa.Models
{
    public class Abonos: BaseEntity
    {
        [Column(Order = 2)]
        public Int64 CreditoId { get; set; }
        public virtual Creditos Credito { get; set; }

        [Column(Order = 3, TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Abono { get; set; }
    }
}
