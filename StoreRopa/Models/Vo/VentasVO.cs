using System.ComponentModel.DataAnnotations;

namespace StoreRopa.Models.Vo
{
    public class VentasVO
    {
        public Ventas? Venta { get; set; } = null;
        public DetalleVentas DetalleVenta { get; set; } = null!;
        public string TipoBusqueda { get; set; } = null!;

        [Display(Name = "Clave de cliente")]
        [Required(ErrorMessage = "La {0} es obligatoria.")]
        [Range(1, Int64.MaxValue, ErrorMessage = "La {0} debe ser de {1} a 99999999" )]
        public Int64 ClaveCliente { get; set; }

        [Required(ErrorMessage = "La {0} es obligatoria.")]
        [StringLength(18)]
        public string Curp { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        public Int64? ClienteId { get; set; } = null!;

        [Display(Name = "Cliente")]
        public string NombreCliente { get; set; } = null!;

        public string TipoVenta { get; set; } = null!;

    }
}
