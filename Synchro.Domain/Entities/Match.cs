namespace Synchro.Domain.Entities
{
    public class Match
    {
        public int IdMatch { get; set; }
        public int IdUsuario1 { get; set; }
        public int IdUsuario2 { get; set; }
        public double? Compatibilidad { get; set; }
        public string? ExplicacionAfinidad { get; set; }
        public DateTime FechaMatch { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string? SugerenciaIA { get; set; }
        public bool Estado { get; set; } = true;
    }
}
