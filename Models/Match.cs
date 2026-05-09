using System;
using Dapper.Contrib.Extensions;

namespace Model
{
    /// <summary>
    /// Representa un match entre dos usuarios
    /// </summary>
    [Table("Match")]
    public class Match
    {
        /// <summary>
        /// Identificador único del match
        /// </summary>
        [Key]
        public int IdMatch { get; set; }

        /// <summary>
        /// Identificador del primer usuario
        /// </summary>
        public int IdUsuario1 { get; set; }

        /// <summary>
        /// Identificador del segundo usuario
        /// </summary>
        public int IdUsuario2 { get; set; }

        /// <summary>
        /// Porcentaje o puntaje de compatibilidad
        /// </summary>
        public double? Compatibilidad { get; set; }

        /// <summary>
        /// Explicación textual de la afinidad
        /// </summary>
        public string? ExplicacionAfinidad { get; set; }

        /// <summary>
        /// Fecha en que se generó el match
        /// </summary>
        public DateTime FechaMatch { get; set; } = DateTime.Now;

        /// <summary>
        /// Fecha de última actualización del match
        /// </summary>
        public DateTime? FechaActualizacion { get; set; }

        /// <summary>
        /// Sugerencia generada por IA
        /// </summary>
        public string? SugerenciaIA { get; set; }

        /// <summary>
        /// Estado activo o inactivo del match
        /// </summary>
        public bool Estado { get; set; } = true;
    }
}