using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreRopa.Models
{
    public class Creditos : BaseEntity
    {
        [Column(Order = 2)]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [Display(Name = "Detalle de venta")]
        public Int64 DetalleVentaId { get; set; }
        public virtual DetalleVentas DetalleVenta { get; set; } = null!;

        [Column(Order = 3, TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio del Articulo")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        public decimal PrecioArticulo { get; set; }

        [Column(Order = 4, TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Pendiente por pagar")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        public decimal PagoPendiente { get; set; }

    }
}
