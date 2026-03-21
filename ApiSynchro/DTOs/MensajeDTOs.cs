namespace ApiSynchro.DTOs
{
    public class MensajeDto
    {
        public int IdMensaje { get; set; }
        public int IdMatch { get; set; }
        public int IdRemitente { get; set; }
        public int IdDestinatario { get; set; }
        public string MensajeTexto { get; set; } = string.Empty;
        public DateTime FechaMensaje { get; set; }
        public string? TipoMensaje { get; set; }
        public bool EstadoLeido { get; set; }
        public string NombreRemitente { get; set; } = string.Empty;
        public string NombreDestinatario { get; set; } = string.Empty;
    }

    public class MensajeCreateDto
    {
        public int IdMatch { get; set; }
        public int IdDestinatario { get; set; }
        public string MensajeTexto { get; set; } = string.Empty;
        public string? TipoMensaje { get; set; }
    }

    public class MensajeResponseDto
    {
        public int IdMensaje { get; set; }
        public int IdMatch { get; set; }
        public int IdRemitente { get; set; }
        public int IdDestinatario { get; set; }
        public string MensajeTexto { get; set; } = string.Empty;
        public DateTime FechaMensaje { get; set; }
        public string? TipoMensaje { get; set; }
        public bool EstadoLeido { get; set; }
    }
}
