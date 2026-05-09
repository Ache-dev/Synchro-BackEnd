using System;
using Dapper.Contrib.Extensions;

namespace Model
{
    /// <summary>
    /// Representa un mensaje enviado entre usuarios dentro de un match
    /// </summary>
    [Table("Mensaje")]
    public class Mensaje
    {
        /// <summary>
        /// Identificador único del mensaje
        /// </summary>
        [Key]
        public int IdMensaje { get; set; }

        /// <summary>
        /// Identificador del match asociado
        /// </summary>
        public int IdMatch { get; set; }

        /// <summary>
        /// Identificador del usuario remitente
        /// </summary>
        public int IdRemitente { get; set; }

        /// <summary>
        /// Identificador del usuario destinatario
        /// </summary>
        public int IdDestinatario { get; set; }

        /// <summary>
        /// Contenido textual del mensaje
        /// </summary>
        public string TextoMensaje { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora en que se envió el mensaje
        /// </summary>
        public DateTime FechaMensaje { get; set; } = DateTime.Now;

        /// <summary>
        /// Tipo de mensaje
        /// </summary>
        public string? TipoMensaje { get; set; }

        /// <summary>
        /// Indica si el mensaje ya fue leído
        /// </summary>
        public bool EstadoLeido { get; set; } = false;
    }
}