using Synchro.Domain.DTOs;
using Synchro.Domain.Entities;
using Synchro.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Synchro.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioResponseDto?> RegistrarUsuarioAsync(UsuarioRegistroDto dto)
        {
            var existe = await _usuarioRepository.GetByEmailAsync(dto.Email);
            if (existe != null) return null;
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                FechaNacimiento = dto.FechaNacimiento,
                Ciudad = dto.Ciudad,
                IntencionBusqueda = dto.IntencionBusqueda,
                Genero = dto.Genero,
                IdiomaPreferido = dto.IdiomaPreferido,
                Estado = true
            };
            usuario.IdUsuario = await _usuarioRepository.InsertAsync(usuario);
            return MapearADto(usuario);
        }

        public async Task<LoginResponseDto?> LoginAsync(UsuarioLoginDto dto)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(dto.Email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Contrasena, usuario.Contrasena))
                return null;
            // Token simulado
            var token = System.Guid.NewGuid().ToString();
            return new LoginResponseDto
            {
                Token = token,
                ExpiraEn = System.DateTime.Now.AddDays(7),
                Usuario = MapearADto(usuario)
            };
        }

        public async Task<UsuarioResponseDto?> ObtenerPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            return usuario != null ? MapearADto(usuario) : null;
        }

        public async Task<List<UsuarioResponseDto>> ObtenerTodosAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            var lista = new List<UsuarioResponseDto>();
            foreach (var u in usuarios)
                lista.Add(MapearADto(u));
            return lista;
        }

        public async Task<UsuarioResponseDto?> ActualizarAsync(int id, UsuarioActualizarDto dto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null) return null;
            if (!string.IsNullOrEmpty(dto.Nombre)) usuario.Nombre = dto.Nombre;
            if (dto.Ciudad != null) usuario.Ciudad = dto.Ciudad;
            if (dto.IntencionBusqueda.HasValue) usuario.IntencionBusqueda = dto.IntencionBusqueda;
            if (dto.Genero != null) usuario.Genero = dto.Genero;
            if (dto.FotoPerfil != null) usuario.FotoPerfil = dto.FotoPerfil;
            if (dto.IdiomaPreferido != null) usuario.IdiomaPreferido = dto.IdiomaPreferido;
            if (dto.TemaPreferido != null) usuario.TemaPreferido = dto.TemaPreferido;
            if (dto.BioAI != null) usuario.BioAI = dto.BioAI;
            await _usuarioRepository.UpdateAsync(usuario);
            return MapearADto(usuario);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _usuarioRepository.DeleteAsync(id);
        }

        public Task CerrarSesionAsync(string token) => Task.CompletedTask;
        public Task<bool> ValidarTokenAsync(string token) => Task.FromResult(true);

        private UsuarioResponseDto MapearADto(Usuario usuario)
        {
            return new UsuarioResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                FechaNacimiento = usuario.FechaNacimiento,
                Ciudad = usuario.Ciudad,
                IntencionBusqueda = usuario.IntencionBusqueda,
                Genero = usuario.Genero,
                FotoPerfil = usuario.FotoPerfil,
                IdiomaPreferido = usuario.IdiomaPreferido,
                TemaPreferido = usuario.TemaPreferido,
                BioAI = usuario.BioAI,
                Estado = usuario.Estado
            };
        }
    }
}
