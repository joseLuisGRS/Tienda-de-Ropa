using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreRopa.Models
{
    public class Persona : BaseEntity
    {
        [Column(Order = 2)]
        [Display(Name = "Nombre(s)")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [StringLength(200)]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [Column(Order = 3)]
        [Display(Name = "Apellido Paterno")]
        [StringLength(100)]
        public string ApPaterno { get; set; }

        [Column(Order = 4)]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [Display(Name = "Apellido Materno")]
        [StringLength(100)]
        public string ApMaterno { get; set;}

        [Column(Order = 5)]
        [Required(ErrorMessage = "La {0} es obligatoria.")]
        [MinLength(18, ErrorMessage = "La {0} debe ser de {1} caracteres")]
        [StringLength(18)]
        public string Curp { get; set;}

        [Column(Order = 6)]
        [Required(ErrorMessage = "La {0} es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [Column(Order = 7)]
        [Required(ErrorMessage = "La {0} es obligatoria.")]
        [Display(Name ="Municipio")]
        [StringLength(500)]
        public string Ciudad { get; set; }

        [Column(Order = 8)]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [Display(Name = "Estado")]
        [StringLength(500)]
        public string Pais { get; set;}

        [Column(Order = 9)]
        [Required(ErrorMessage = "La {0} es obligatoria.")]
        [Display(Name = "Dirección")]
        [StringLength(1000)]
        public string Direccion { get; set;}

        [Column(Order = 10)]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [Display(Name = "Número")]
        [StringLength(10)]
        public string Numero { get; set;}

        [Column(Order = 11)]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [Display(Name = "Código postal")]
        [DataType(DataType.PostalCode, ErrorMessage = "El {0} no es valido")]
        public int? Cp { get; set;}

        [Column(Order = 12)]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [MinLength(10, ErrorMessage = "El {0} debe ser de {1} dígitos")]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "El {0} no es valido")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set;}

        public virtual Cliente Cliente { get; set; } = null!;

        public static implicit operator Persona(List<Persona> v)
        {
            throw new NotImplementedException();
        }
    }
}
