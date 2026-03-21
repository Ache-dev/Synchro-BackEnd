namespace Synchro.Domain.DTOs
{
    public class EncuestaResponseDto
    {
        public int IdPregunta { get; set; }
        public string TextoPregunta { get; set; } = string.Empty;
        public string? TextoPreguntaEN { get; set; }
        public string? Icono { get; set; }
        public int Orden { get; set; }
    }
    public class EncuestaCreateDto
    {
        public string TextoPregunta { get; set; } = string.Empty;
        public string? TextoPreguntaEN { get; set; }
        public string? Icono { get; set; }
        public int Orden { get; set; }
    }
}
