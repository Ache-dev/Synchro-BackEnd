using Dapper.Contrib.Extensions;

namespace Model
{
    /// <summary>
    /// Representa la respuesta de un usuario a una pregunta de encuesta
    /// </summary>
    [Table("RespuestaEncuesta")]
    public class RespuestaEncuesta
    {
        /// <summary>
        /// Identificador único de la respuesta
        /// </summary>
        [Key]
        public int IdRespuesta { get; set; }

        /// <summary>
        /// Identificador del usuario que respondió
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Identificador de la pregunta respondida
        /// </summary>
        public int IdPregunta { get; set; }

        /// <summary>
        /// Texto libre de la respuesta
        /// </summary>
        public string RespuestaTexto { get; set; } = string.Empty;
    }
}