using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSynchro.Models
{
    [Table("RespuestaEncuesta")]
    public class RespuestaEncuesta
    {
        [Key]
        public int IdRespuesta { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public int IdPregunta { get; set; }

        [Required]
        [MaxLength(500)]
        public string RespuestaTexto { get; set; } = string.Empty;

        [ForeignKey("IdUsuario")]
        public virtual Usuario? Usuario { get; set; }

        [ForeignKey("IdPregunta")]
        public virtual PreguntaEncuesta? Pregunta { get; set; }
    }
}
