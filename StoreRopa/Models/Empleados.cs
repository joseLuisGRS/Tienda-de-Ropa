using StackExchange.Redis;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StoreRopa.Models
{
    public class Empleados : BaseEntity 
    {
        [Column(Order = 2)]
        public Int64 PersonaId { get; set; }
        public virtual Persona Persona { get; set; } = null!;
        [Column(Order = 3)]
        public Int64 RolId { get; set; }
        public virtual Roles Rol { get; set; } = null!;

        [Column(Order = 4)]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [MinLength(5, ErrorMessage = "El {0} debe ser de {1} caracteres como mínimo.")]
        [StringLength(50)]
        public string Usuario { get; set; }

        [Column(Order = 5)]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La {0} es obligatoria.")]
        [StringLength(100)]
        public string Password { get; set; }
    }
}
