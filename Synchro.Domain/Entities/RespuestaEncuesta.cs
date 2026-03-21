namespace Synchro.Domain.Entities
{
    public class RespuestaEncuesta
    {
        public int IdRespuesta { get; set; }
        public int IdUsuario { get; set; }
        public int IdPregunta { get; set; }
        public string RespuestaTexto { get; set; } = string.Empty;
    }
}
