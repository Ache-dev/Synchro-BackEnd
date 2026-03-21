namespace Synchro.Domain.DTOs
{
    public class IntencionBusquedaResponseDto
    {
        public int IdIntencion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? NombreEN { get; set; }
    }
    public class IntencionBusquedaCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? NombreEN { get; set; }
    }
}
