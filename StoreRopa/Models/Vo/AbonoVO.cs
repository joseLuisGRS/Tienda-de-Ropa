using System.ComponentModel.DataAnnotations;

namespace StoreRopa.Models.Vo
{
    public class AbonoVO
    {
        public string TipoBusqueda { get; set; } = null!;

        [Display(Name = "Clave de cliente")]
        [Required(ErrorMessage = "La {0} es obligatoria.")]
        [Range(1, Int64.MaxValue, ErrorMessage = "La {0} debe ser de {1} a 99999999")]
        public Int64 ClaveCliente { get; set; }

        [Required(ErrorMessage = "La {0} es obligatoria.")]
        [StringLength(18)]
        public string Curp { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        public Int64? ClienteId { get; set; } = null!;

        public Int64 AbonoClienteId { get; set; }

        [Display(Name = "Cliente")]
        public string NombreCliente { get; set; } = null!;

        public decimal Saldo { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        public decimal Abono { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Cantidad Recibida")]
        [Required(ErrorMessage = "La {0} es obligatoria.")]
        public decimal CantidadRecibida { get; set; } 

        public decimal PendientePago { get; set; }
    }
}
