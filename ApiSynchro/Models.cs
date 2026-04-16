using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSynchro.Models
{
    /// <summary>
    /// Representa una intención de búsqueda disponible para los usuarios.
    /// </summary>
    [Table("IntencionBusqueda")]
    public class IntencionBusqueda
    {
        /// <summary>
        /// Identificador único de la intención.
        /// </summary>
        [Key]
        public int IdIntencion { get; set; }

        /// <summary>
        /// Nombre principal de la intención.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de la intención en inglés.
        /// </summary>
        [MaxLength(100)]
        public string? NombreEN { get; set; }

        /// <summary>
        /// Colección de usuarios asociados a esta intención.
        /// </summary>
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }

    /// <summary>
    /// Representa un usuario registrado en la plataforma.
    /// </summary>
    [Table("Usuario")]
    public class Usuario
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        [Key]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Nombre visible del usuario.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña almacenada en formato hash.
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string Contrasena { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de nacimiento del usuario.
        /// </summary>
        [Required]
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Ciudad de residencia del usuario.
        /// </summary>
        [MaxLength(150)]
        public string? Ciudad { get; set; }

        /// <summary>
        /// Identificador de la intención de búsqueda seleccionada.
        /// </summary>
        public int? IntencionBusqueda { get; set; }

        /// <summary>
        /// Género declarado por el usuario.
        /// </summary>
        [MaxLength(20)]
        public string? Genero { get; set; }

        /// <summary>
        /// Navegación a la intención de búsqueda asociada.
        /// </summary>
        [ForeignKey("IntencionBusqueda")]
        public virtual IntencionBusqueda? IntencionBusquedaNavigation { get; set; }

        /// <summary>
        /// URL o referencia de foto de perfil.
        /// </summary>
        [MaxLength(500)]
        public string? FotoPerfil { get; set; }

        /// <summary>
        /// Idioma preferido para la experiencia de uso.
        /// </summary>
        [MaxLength(5)]
        public string? IdiomaPreferido { get; set; }

        /// <summary>
        /// Tema visual preferido por el usuario.
        /// </summary>
        [MaxLength(10)]
        public string? TemaPreferido { get; set; }

        /// <summary>
        /// Biografía generada por IA o personalizada.
        /// </summary>
        [MaxLength(500)]
        public string? BioAI { get; set; }

        /// <summary>
        /// Vector de embedding del perfil serializado.
        /// </summary>
        [MaxLength(500)]
        public string? EmbeddingPerfil { get; set; }

        /// <summary>
        /// Indica si el usuario está activo.
        /// </summary>
        public bool Estado { get; set; } = true;

        /// <summary>
        /// Sesiones activas o históricas del usuario.
        /// </summary>
        public virtual ICollection<Sesion> Sesiones { get; set; } = new List<Sesion>();

        /// <summary>
        /// Respuestas de encuesta registradas por el usuario.
        /// </summary>
        public virtual ICollection<RespuestaEncuesta> Respuestas { get; set; } = new List<RespuestaEncuesta>();

        /// <summary>
        /// Matches donde el usuario participa como primer miembro.
        /// </summary>
        public virtual ICollection<Match> MatchesComoUsuario1 { get; set; } = new List<Match>();

        /// <summary>
        /// Matches donde el usuario participa como segundo miembro.
        /// </summary>
        public virtual ICollection<Match> MatchesComoUsuario2 { get; set; } = new List<Match>();

        /// <summary>
        /// Mensajes enviados por el usuario.
        /// </summary>
        public virtual ICollection<Mensaje> MensajesEnviados { get; set; } = new List<Mensaje>();

        /// <summary>
        /// Mensajes recibidos por el usuario.
        /// </summary>
        public virtual ICollection<Mensaje> MensajesRecibidos { get; set; } = new List<Mensaje>();
    }

    /// <summary>
    /// Representa una sesión autenticada de usuario.
    /// </summary>
    [Table("Sesion")]
    public class Sesion
    {
        /// <summary>
        /// Identificador único de la sesión.
        /// </summary>
        [Key]
        public int IdSesion { get; set; }

        /// <summary>
        /// Identificador del usuario propietario de la sesión.
        /// </summary>
        [Required]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Token único de autenticación de la sesión.
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de expiración de la sesión.
        /// </summary>
        [Required]
        public DateTime ExpiraEn { get; set; }

        /// <summary>
        /// Fecha y hora de creación de la sesión.
        /// </summary>
        public DateTime CreadoEn { get; set; } = DateTime.Now;

        /// <summary>
        /// Navegación al usuario propietario de la sesión.
        /// </summary>
        [ForeignKey("IdUsuario")]
        public virtual Usuario? Usuario { get; set; }
    }

    /// <summary>
    /// Define una pregunta de encuesta para perfilado de usuarios.
    /// </summary>
    [Table("PreguntaEncuesta")]
    public class PreguntaEncuesta
    {
        /// <summary>
        /// Identificador único de la pregunta.
        /// </summary>
        [Key]
        public int IdPregunta { get; set; }

        /// <summary>
        /// Texto principal de la pregunta.
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string TextoPregunta { get; set; } = string.Empty;

        /// <summary>
        /// Texto alternativo de la pregunta en inglés.
        /// </summary>
        [MaxLength(500)]
        public string? TextoPreguntaEN { get; set; }

        /// <summary>
        /// Identificador visual o ícono representativo.
        /// </summary>
        [MaxLength(50)]
        public string? Icono { get; set; }

        /// <summary>
        /// Orden de presentación de la pregunta.
        /// </summary>
        [Required]
        public int Orden { get; set; }

        /// <summary>
        /// Respuestas asociadas a esta pregunta.
        /// </summary>
        public virtual ICollection<RespuestaEncuesta> Respuestas { get; set; } = new List<RespuestaEncuesta>();
    }

    /// <summary>
    /// Representa la respuesta de un usuario a una pregunta de encuesta.
    /// </summary>
    [Table("RespuestaEncuesta")]
    public class RespuestaEncuesta
    {
        /// <summary>
        /// Identificador único de la respuesta.
        /// </summary>
        [Key]
        public int IdRespuesta { get; set; }

        /// <summary>
        /// Identificador del usuario que responde.
        /// </summary>
        [Required]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Identificador de la pregunta respondida.
        /// </summary>
        [Required]
        public int IdPregunta { get; set; }

        /// <summary>
        /// Contenido textual de la respuesta.
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string RespuestaTexto { get; set; } = string.Empty;

        /// <summary>
        /// Navegación al usuario que emitió la respuesta.
        /// </summary>
        [ForeignKey("IdUsuario")]
        public virtual Usuario? Usuario { get; set; }

        /// <summary>
        /// Navegación a la pregunta asociada.
        /// </summary>
        [ForeignKey("IdPregunta")]
        public virtual PreguntaEncuesta? Pregunta { get; set; }
    }

    /// <summary>
    /// Representa una relación de afinidad entre dos usuarios.
    /// </summary>
    [Table("Match")]
    public class Match
    {
        /// <summary>
        /// Identificador único del match.
        /// </summary>
        [Key]
        public int IdMatch { get; set; }

        /// <summary>
        /// Identificador del primer usuario.
        /// </summary>
        [Required]
        public int IdUsuario1 { get; set; }

        /// <summary>
        /// Identificador del segundo usuario.
        /// </summary>
        [Required]
        public int IdUsuario2 { get; set; }

        /// <summary>
        /// Porcentaje de compatibilidad calculado.
        /// </summary>
        public double? Compatibilidad { get; set; }

        /// <summary>
        /// Explicación textual de la afinidad detectada.
        /// </summary>
        [MaxLength(500)]
        public string? ExplicacionAfinidad { get; set; }

        /// <summary>
        /// Fecha de creación del match.
        /// </summary>
        public DateTime FechaMatch { get; set; } = DateTime.Now;

        /// <summary>
        /// Fecha de última actualización del match.
        /// </summary>
        public DateTime? FechaActualizacion { get; set; }

        /// <summary>
        /// Sugerencia generada por IA para iniciar la conversación.
        /// </summary>
        [MaxLength(500)]
        public string? SugerenciaIA { get; set; }

        /// <summary>
        /// Indica si el match está activo.
        /// </summary>
        public bool Estado { get; set; } = true;

        /// <summary>
        /// Navegación al primer usuario del match.
        /// </summary>
        [ForeignKey("IdUsuario1")]
        public virtual Usuario? Usuario1 { get; set; }

        /// <summary>
        /// Navegación al segundo usuario del match.
        /// </summary>
        [ForeignKey("IdUsuario2")]
        public virtual Usuario? Usuario2 { get; set; }

        /// <summary>
        /// Colección de mensajes asociados al match.
        /// </summary>
        public virtual ICollection<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
    }

    /// <summary>
    /// Representa un mensaje intercambiado dentro de un match.
    /// </summary>
    [Table("Mensaje")]
    public class Mensaje
    {
        /// <summary>
        /// Identificador único del mensaje.
        /// </summary>
        [Key]
        public int IdMensaje { get; set; }

        /// <summary>
        /// Identificador del match asociado.
        /// </summary>
        [Required]
        public int IdMatch { get; set; }

        /// <summary>
        /// Identificador del usuario remitente.
        /// </summary>
        [Required]
        public int IdRemitente { get; set; }

        /// <summary>
        /// Identificador del usuario destinatario.
        /// </summary>
        [Required]
        public int IdDestinatario { get; set; }

        /// <summary>
        /// Contenido textual del mensaje.
        /// </summary>
        [Required]
        [MaxLength(500)]
        [Column("Mensaje")]
        public string MensajeTexto { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de envío del mensaje.
        /// </summary>
        public DateTime FechaMensaje { get; set; } = DateTime.Now;

        /// <summary>
        /// Tipo de mensaje (texto, sistema u otro).
        /// </summary>
        [MaxLength(20)]
        public string? TipoMensaje { get; set; }

        /// <summary>
        /// Indica si el destinatario ya leyó el mensaje.
        /// </summary>
        public bool EstadoLeido { get; set; } = false;

        /// <summary>
        /// Navegación al match al que pertenece el mensaje.
        /// </summary>
        [ForeignKey("IdMatch")]
        public virtual Match? Match { get; set; }

        /// <summary>
        /// Navegación al usuario remitente.
        /// </summary>
        [ForeignKey("IdRemitente")]
        public virtual Usuario? Remitente { get; set; }

        /// <summary>
        /// Navegación al usuario destinatario.
        /// </summary>
        [ForeignKey("IdDestinatario")]
        public virtual Usuario? Destinatario { get; set; }
    }
}
