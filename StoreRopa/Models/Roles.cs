using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreRopa.Models
{
    public class Roles: BaseEntity
    {
        [Column(Order = 2)]
        [Required(ErrorMessage = "El {0} del rol es obligatorio.")]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Column(Order = 3)]
        [Required(ErrorMessage = "La {0} del rol es obligatoria.")]
        [StringLength(100)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public virtual ICollection<Empleados> Empleados { get; set; }
    }
}