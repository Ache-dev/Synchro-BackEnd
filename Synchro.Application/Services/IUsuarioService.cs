using Synchro.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Synchro.Application.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioResponseDto?> RegistrarUsuarioAsync(UsuarioRegistroDto dto);
        Task<LoginResponseDto?> LoginAsync(UsuarioLoginDto dto);
        Task<UsuarioResponseDto?> ObtenerPorIdAsync(int id);
        Task<List<UsuarioResponseDto>> ObtenerTodosAsync();
        Task<UsuarioResponseDto?> ActualizarAsync(int id, UsuarioActualizarDto dto);
        Task<bool> EliminarAsync(int id);
        Task CerrarSesionAsync(string token);
        Task<bool> ValidarTokenAsync(string token);
    }
}
