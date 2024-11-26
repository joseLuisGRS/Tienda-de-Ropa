using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreRopa.Models
{
    public class DetalleVentas : BaseEntity
    {
        [Column(Order = 2)]
        public Int64 VentaId { get; set; }
        public virtual Ventas Venta { get; set; } = null!;

        [Column(Order = 3)]
        [Display(Name = "Articulo")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [StringLength(1000)]
        public string Descripcion { get; set; }

        [Column(Order = 4)]
        [Required(ErrorMessage = "La {0} es obligatoria.")]
        [StringLength(50)]
        public string Talla { get; set; }

        [Column(Order = 5)]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [StringLength(100)]
        public string Color { get; set; }

        [Column(Order = 6)]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [StringLength(50)]
        public string Modelo { get; set; }

        [Column(Order = 7, TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio del Articulo")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        public decimal PrecioArticulo { get; set; }

        [Column(Order = 8, TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        public decimal Descuento { get; set; }

        [Column(Order = 9, TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [Display(Name = "Precio de venta")]
        public decimal PrecioVenta { get; set; }

        public virtual ICollection<Creditos> Creditos { get; set; }

        public virtual Devoluciones Devolucion { get; set; }


    }
}
