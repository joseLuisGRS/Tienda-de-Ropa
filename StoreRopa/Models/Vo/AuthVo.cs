using System.ComponentModel.DataAnnotations;

namespace StoreRopa.Models.Vo
{
    public class AuthVo
    {
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "La {0} es obligatoria.")]
        public string Password { get; set; }
    }
}
