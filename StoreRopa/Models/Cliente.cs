using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreRopa.Models
{
    public class Cliente
    {
        [Column(Order = 1)]
        public Int64 Id { get; set; }
        [Column(Order = 2)]
        public Int64 PersonaId { get; set; }
        public virtual Persona Persona { get; set; } = null!;
        [Column(Order = 3)]
        public double Saldo { get; set; }
        [Column(Order = 4)]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [Display(Name = "Tipo de venta")]
        public int? TipoVenta { get; set; }
        [Column(Order = 5)]
        public bool EsActivo { get; set; }
        [Column(Order = 6)]
        public bool EsEliminado { get; set; }
    }
}
