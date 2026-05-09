using System;
using Dapper.Contrib.Extensions;

namespace Model
{
    /// <summary>
    /// Representa una sesión de autenticación de usuario
    /// </summary>
    [Table("Sesion")]
    public class Sesion
    {
        /// <summary>
        /// Identificador único de la sesión
        /// </summary>
        [Key]
        public int IdSesion { get; set; }

        /// <summary>
        /// Identificador del usuario asociado a la sesión
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Token de sesión generado
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de expiración de la sesión
        /// </summary>
        public DateTime ExpiraEn { get; set; }

        /// <summary>
        /// Fecha y hora de creación de la sesión
        /// </summary>
        public DateTime CreadoEn { get; set; } = DateTime.Now;
    }
}