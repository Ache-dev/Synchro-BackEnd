using System;
using Dapper.Contrib.Extensions;

namespace Model
{
    /// <summary>
    /// Clase que representa los usuarios registrados en el sistema
    /// </summary>
    [Table("Usuario")]
    public class Usuario
    {
        /// <summary>
        /// Id del usuario (Identity)
        /// </summary>
        [Key]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico institucional o personal
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña cifrada del usuario
        /// </summary>
        public string Contrasena { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de nacimiento para validación de edad
        /// </summary>
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Ciudad de residencia
        /// </summary>
        public string? Ciudad { get; set; }

        /// <summary>
        /// Referencia a la intención de búsqueda
        /// </summary>
        public int? IntencionBusqueda { get; set; }

        /// <summary>
        /// Género del usuario
        /// </summary>
        public string? Genero { get; set; }

        /// <summary>
        /// Ruta o URL de la foto de perfil
        /// </summary>
        public string? FotoPerfil { get; set; }

        /// <summary>
        /// Código de idioma (ej. ES, EN)
        /// </summary>
        public string? IdiomaPreferido { get; set; }

        /// <summary>
        /// Preferencia de tema (Light/Dark)
        /// </summary>
        public string? TemaPreferido { get; set; }

        /// <summary>
        /// Biografía analizada por la IA
        /// </summary>
        public string? BioAi { get; set; }

        /// <summary>
        /// Vector de perfil serializado para algoritmos de coincidencia
        /// </summary>
        public string? EmbeddingPerfil { get; set; }

        /// <summary>
        /// Estado del usuario, true activo (1), false inactivo (0)
        /// </summary>
        public bool Estado { get; set; } = true;
    }
}