namespace ApiSynchro.DTOs
{
    public class IntencionBusquedaResponseDto
    {
        public int IdIntencion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? NombreEN { get; set; }
    }

    public class IntencionBusquedaCreateDto
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El nombre es requerido")]
        [System.ComponentModel.DataAnnotations.MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.MaxLength(100, ErrorMessage = "El nombre en inglés no puede exceder 100 caracteres")]
        public string? NombreEN { get; set; }
    }

    public class IntencionBusquedaUpdateDto
    {
        [System.ComponentModel.DataAnnotations.MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? Nombre { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength(100, ErrorMessage = "El nombre en inglés no puede exceder 100 caracteres")]
        public string? NombreEN { get; set; }
    }
}
