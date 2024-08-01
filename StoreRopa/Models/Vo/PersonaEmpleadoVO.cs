using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StoreRopa.Models.Vo
{
    public class PersonaEmpleadoVO
    {
        public PersonaEmpleadoVO() { }
        public PersonaEmpleadoVO(List<Roles> roles) {
            this.roles = roles;
        }
        public PersonaEmpleadoVO(Persona persona, Empleados empleado, Int64? rolID, List<Roles> roles) {
            this.persona = persona;
            this.empleado = empleado;
            this.RolId = rolID;
            this.roles = roles;
        }
        public Persona persona { get; set; } = null!;
        public Empleados empleado { get; set; } = null!;
        [Required(ErrorMessage = "El rol es obligatorio.")]
        [Display(Name = "Rol")]
        public Int64? RolId { get; set; } = null!;

        [Required(ErrorMessage = "La Contraseña es obligatoria.")]
        [Display(Name = "Conformación de Contraseña" )]
        public string ConfirmaPwd { get; set; } = null!;
        public List<Roles> roles { get; set; } = null!;

    }
}
