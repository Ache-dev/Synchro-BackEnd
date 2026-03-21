using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSynchro.Models
{
    [Table("Sesion")]
    public class Sesion
    {
        [Key]
        public int IdSesion { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(500)]
        public string Token { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiraEn { get; set; }

        public DateTime CreadoEn { get; set; } = DateTime.Now;

        [ForeignKey("IdUsuario")]
        public virtual Usuario? Usuario { get; set; }
    }
}
