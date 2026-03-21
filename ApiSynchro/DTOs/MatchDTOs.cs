namespace ApiSynchro.DTOs
{
    public class MatchDto
    {
        public int IdMatch { get; set; }
        public int IdUsuario1 { get; set; }
        public int IdUsuario2 { get; set; }
        public double? Compatibilidad { get; set; }
        public string? ExplicacionAfinidad { get; set; }
        public DateTime FechaMatch { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string? SugerenciaIA { get; set; }
        public bool Estado { get; set; }
        public UsuarioResponseDto? Usuario1 { get; set; }
        public UsuarioResponseDto? Usuario2 { get; set; }
    }

    public class MatchCreateDto
    {
        public int IdUsuario2 { get; set; }
        public double? Compatibilidad { get; set; }
        public string? ExplicacionAfinidad { get; set; }
        public string? SugerenciaIA { get; set; }
    }

    public class MatchActualizarDto
    {
        public double? Compatibilidad { get; set; }
        public string? ExplicacionAfinidad { get; set; }
        public string? SugerenciaIA { get; set; }
        public bool? Estado { get; set; }
    }
}
