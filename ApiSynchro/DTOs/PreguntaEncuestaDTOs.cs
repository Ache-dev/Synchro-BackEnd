namespace ApiSynchro.DTOs
{
    public class PreguntaEncuestaDto
    {
        public int IdPregunta { get; set; }
        public string TextoPregunta { get; set; } = string.Empty;
        public string? TextoPreguntaEN { get; set; }
        public string? Icono { get; set; }
        public int Orden { get; set; }
    }

    public class PreguntaEncuestaCreateDto
    {
        public string TextoPregunta { get; set; } = string.Empty;
        public string? TextoPreguntaEN { get; set; }
        public string? Icono { get; set; }
        public int Orden { get; set; }
    }
}
