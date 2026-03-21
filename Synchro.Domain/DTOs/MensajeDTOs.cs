namespace Synchro.Domain.DTOs
{
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
    public class MensajeCreateDto
    {
        public int IdMatch { get; set; }
        public int IdRemitente { get; set; }
        public int IdDestinatario { get; set; }
        public string MensajeTexto { get; set; } = string.Empty;
    }
}
