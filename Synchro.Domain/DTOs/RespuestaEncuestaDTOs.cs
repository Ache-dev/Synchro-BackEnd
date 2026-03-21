namespace Synchro.Domain.DTOs
{
    public class RespuestaEncuestaResponseDto
    {
        public int IdRespuesta { get; set; }
        public int IdUsuario { get; set; }
        public int IdPregunta { get; set; }
        public string RespuestaTexto { get; set; } = string.Empty;
    }
    public class RespuestaEncuestaCreateDto
    {
        public int IdUsuario { get; set; }
        public int IdPregunta { get; set; }
        public string RespuestaTexto { get; set; } = string.Empty;
    }
}
