using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSynchro.Models
{
    [Table("IntencionBusqueda")]
    public class IntencionBusqueda
    {
        [Key]
        public int IdIntencion { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? NombreEN { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }

    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string Contrasena { get; set; } = string.Empty;

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [MaxLength(150)]
        public string? Ciudad { get; set; }

        public int? IntencionBusqueda { get; set; }

        [MaxLength(20)]
        public string? Genero { get; set; }

        [ForeignKey("IntencionBusqueda")]
        public virtual IntencionBusqueda? IntencionBusquedaNavigation { get; set; }

        [MaxLength(500)]
        public string? FotoPerfil { get; set; }

        [MaxLength(5)]
        public string? IdiomaPreferido { get; set; }

        [MaxLength(10)]
        public string? TemaPreferido { get; set; }

        [MaxLength(500)]
        public string? BioAI { get; set; }

        [MaxLength(500)]
        public string? EmbeddingPerfil { get; set; }

        public bool Estado { get; set; } = true;

        public virtual ICollection<Sesion> Sesiones { get; set; } = new List<Sesion>();
        public virtual ICollection<RespuestaEncuesta> Respuestas { get; set; } = new List<RespuestaEncuesta>();
        public virtual ICollection<Match> MatchesComoUsuario1 { get; set; } = new List<Match>();
        public virtual ICollection<Match> MatchesComoUsuario2 { get; set; } = new List<Match>();
        public virtual ICollection<Mensaje> MensajesEnviados { get; set; } = new List<Mensaje>();
        public virtual ICollection<Mensaje> MensajesRecibidos { get; set; } = new List<Mensaje>();
    }

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
        [Column("Mensaje")]
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
