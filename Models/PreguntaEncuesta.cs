using Dapper.Contrib.Extensions;

namespace Model
{
    /// <summary>
    /// Representa una pregunta de encuesta disponible en el sistema
    /// </summary>
    [Table("PreguntaEncuesta")]
    public class PreguntaEncuesta
    {
        /// <summary>
        /// Identificador único de la pregunta
        /// </summary>
        [Key]
        public int IdPregunta { get; set; }

        /// <summary>
        /// Texto principal de la pregunta
        /// </summary>
        public string TextoPregunta { get; set; } = string.Empty;

        /// <summary>
        /// Texto de la pregunta en inglés
        /// </summary>
        public string? TextoPreguntaEN { get; set; }

        /// <summary>
        /// Icono representativo de la pregunta
        /// </summary>
        public string? Icono { get; set; }

        /// <summary>
        /// Orden de visualización de la pregunta
        /// </summary>
        public int Orden { get; set; }
    }
}