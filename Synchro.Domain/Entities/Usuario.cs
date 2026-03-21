using System;

namespace Synchro.Domain.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string? Ciudad { get; set; }
        public int? IntencionBusqueda { get; set; }
        public string? Genero { get; set; }
        public string? FotoPerfil { get; set; }
        public string? IdiomaPreferido { get; set; }
        public string? TemaPreferido { get; set; }
        public string? BioAI { get; set; }
        public bool Estado { get; set; } = true;
    }
}
