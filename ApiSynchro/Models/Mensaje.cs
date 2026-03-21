using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSynchro.Models
{
    [Table("Mensaje")]
    public class Mensaje
    {
        [Key]
        public int IdMensaje { get; set; }

        [Required]
        public int IdMatch { get; set; }

        [Required]
        public int IdRemitente { get; set; }

        [Required]
        public int IdDestinatario { get; set; }

        [Required]
        [MaxLength(500)]
        public string MensajeTexto { get; set; } = string.Empty;

        public DateTime FechaMensaje { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string? TipoMensaje { get; set; }

        public bool EstadoLeido { get; set; } = false;

        [ForeignKey("IdMatch")]
        public virtual Match? Match { get; set; }

        [ForeignKey("IdRemitente")]
        public virtual Usuario? Remitente { get; set; }

        [ForeignKey("IdDestinatario")]
        public virtual Usuario? Destinatario { get; set; }
    }
}
