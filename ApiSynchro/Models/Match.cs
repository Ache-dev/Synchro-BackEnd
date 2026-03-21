using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSynchro.Models
{
    [Table("Match")]
    public class Match
    {
        [Key]
        public int IdMatch { get; set; }

        [Required]
        public int IdUsuario1 { get; set; }

        [Required]
        public int IdUsuario2 { get; set; }

        public double? Compatibilidad { get; set; }

        [MaxLength(500)]
        public string? ExplicacionAfinidad { get; set; }

        public DateTime FechaMatch { get; set; } = DateTime.Now;

        public DateTime? FechaActualizacion { get; set; }

        [MaxLength(500)]
        public string? SugerenciaIA { get; set; }

        public bool Estado { get; set; } = true;

        [ForeignKey("IdUsuario1")]
        public virtual Usuario? Usuario1 { get; set; }

        [ForeignKey("IdUsuario2")]
        public virtual Usuario? Usuario2 { get; set; }

        public virtual ICollection<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
    }
}
