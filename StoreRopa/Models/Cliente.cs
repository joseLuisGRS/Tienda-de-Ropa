using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreRopa.Models
{
    public class Cliente : BaseEntity
    {
        [Column(Order = 2)]
        public Int64 PersonaId { get; set; }
        public virtual Persona Persona { get; set; } = null!;
        [Column(Order = 3, TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Saldo { get; set; }
        [Column(Order = 4)]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [Display(Name = "Tipo de venta")]
        public int? TipoVenta { get; set; }
        public virtual ICollection<Ventas> Ventas { get; set; }

    }
}
