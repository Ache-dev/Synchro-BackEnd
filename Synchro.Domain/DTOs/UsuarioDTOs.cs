namespace Synchro.Domain.DTOs
{
    public class UsuarioRegistroDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string? Ciudad { get; set; }
        public int? IntencionBusqueda { get; set; }
        public string? Genero { get; set; }
        public string? IdiomaPreferido { get; set; }
    }

    public class UsuarioLoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
    }

    public class UsuarioResponseDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string? Ciudad { get; set; }
        public int? IntencionBusqueda { get; set; }
        public string? Genero { get; set; }
        public string? FotoPerfil { get; set; }
        public string? IdiomaPreferido { get; set; }
        public string? TemaPreferido { get; set; }
        public string? BioAI { get; set; }
        public bool Estado { get; set; }
    }

    public class UsuarioActualizarDto
    {
        public string? Nombre { get; set; }
        public string? Ciudad { get; set; }
        public int? IntencionBusqueda { get; set; }
        public string? Genero { get; set; }
        public string? FotoPerfil { get; set; }
        public string? IdiomaPreferido { get; set; }
        public string? TemaPreferido { get; set; }
        public string? BioAI { get; set; }
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiraEn { get; set; }
        public UsuarioResponseDto Usuario { get; set; } = null!;
    }
}
