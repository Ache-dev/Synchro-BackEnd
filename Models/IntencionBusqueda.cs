using Dapper.Contrib.Extensions;

namespace Model
{
    /// <summary>
    /// Representa una intención de búsqueda del usuario
    /// </summary>
    [Table("IntencionBusqueda")]
    public class IntencionBusqueda
    {
        /// <summary>
        /// Identificador único de la intención
        /// </summary>
        [Key]
        public int IdIntencion { get; set; }

        /// <summary>
        /// Nombre de la intención en el idioma principal
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de la intención en inglés
        /// </summary>
        public string? NombreEN { get; set; }
    }
}