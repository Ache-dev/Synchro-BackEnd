namespace ApiSynchro.DTOs
{
    public class RespuestaEncuestaDto
    {
        public int IdRespuesta { get; set; }
        public int IdUsuario { get; set; }
        public int IdPregunta { get; set; }
        public string RespuestaTexto { get; set; } = string.Empty;
    }

    public class RespuestaEncuestaCreateDto
    {
        public int IdPregunta { get; set; }
        public string RespuestaTexto { get; set; } = string.Empty;
    }

    public class RespuestasEncuestaCreateDto
    {
        public List<RespuestaEncuestaCreateDto> Respuestas { get; set; } = new();
    }
}
