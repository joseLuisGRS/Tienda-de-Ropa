using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Serialization;

namespace StoreRopa.Models
{
    public class Ventas : BaseEntity
    {
        [Column(Order = 2)]
        [Display(Name = "Clave")]
        [Required(ErrorMessage = "El Cliente es obligatorio.")]
        public Int64 ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; } = null!;

        [Column(Order = 3)]
        public Int64 EmpleadoId { get; set; }
        public virtual Empleados Empleado { get; set; } = null!;

        [Column(Order = 4, TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Total a pagar")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        public decimal ImporteVenta { get; set; }

        [Column(Order = 5, TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Abono / Pago")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        public decimal AbonoVenta { get; set; }

        [Column(Order = 6, TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Por Pagar")]
        public decimal PendientePago { get; set; }

        [Column(Order = 7)]
        public bool EsVentaCredito { get; set; }

        public virtual ICollection<DetalleVentas> DetalleVentas { get; set; }
    }
}
