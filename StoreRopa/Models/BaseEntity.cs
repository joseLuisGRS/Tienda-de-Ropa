using System.ComponentModel.DataAnnotations.Schema;

namespace StoreRopa.Models
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// Indentificador unico de la tabla
        /// </summary>
        [Column(Order = 1)]
        public Int64 Id { get; set; }
        /// <summary>
        /// Indica si esta activo el registro
        /// </summary>
        [Column(Order = 101)]
        public bool EsActivo { get; set; }

        /// <summary>
        /// Indica si esta eliminado el registro
        /// </summary>
        [Column(Order = 102)]
        public bool EsEliminado { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        [Column(Order = 103)]
        public DateTime FechaAlta {  get ;  set; }

        /// <summary>
        /// Identificador del usuario que creo el registro
        /// </summary>
        [Column(Order = 104)]
        public string? UsuarioAlta { get; set; }

        /// <summary>
        /// Fecha de la ultima actualización del registro
        /// </summary>
        [Column(Order = 105)]
        public DateTime? FechaModificacion { get; set; }

        /// <summary>
        /// Identificador del ultimo usuario que actualizo el registro
        /// </summary>
        [Column(Order = 106)]
        public string? UsuarioModificacion { get; set; }
    }
}
