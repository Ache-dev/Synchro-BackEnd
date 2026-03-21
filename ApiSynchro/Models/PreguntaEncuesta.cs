using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSynchro.Models
{
    [Table("PreguntaEncuesta")]
    public class PreguntaEncuesta
    {
        [Key]
        public int IdPregunta { get; set; }

        [Required]
        [MaxLength(500)]
        public string TextoPregunta { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? TextoPreguntaEN { get; set; }

        [MaxLength(50)]
        public string? Icono { get; set; }

        [Required]
        public int Orden { get; set; }

        public virtual ICollection<RespuestaEncuesta> Respuestas { get; set; } = new List<RespuestaEncuesta>();
    }
}
