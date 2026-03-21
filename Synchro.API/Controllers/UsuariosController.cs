using Microsoft.AspNetCore.Mvc;
using Synchro.Application.Services;
using Synchro.Domain.DTOs;

namespace Synchro.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("registro")]
        public async Task<ActionResult<UsuarioResponseDto>> Registrar([FromBody] UsuarioRegistroDto dto)
        {
            var usuario = await _usuarioService.RegistrarUsuarioAsync(dto);
            if (usuario == null)
                return BadRequest(new { mensaje = "El email ya está registrado" });
            return CreatedAtAction(nameof(ObtenerPorId), new { id = usuario.IdUsuario }, usuario);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] UsuarioLoginDto dto)
        {
            var resultado = await _usuarioService.LoginAsync(dto);
            if (resultado == null)
                return Unauthorized(new { mensaje = "Credenciales inválidas" });
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> ObtenerPorId(int id)
        {
            var usuario = await _usuarioService.ObtenerPorIdAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });
            return Ok(usuario);
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioResponseDto>>> ObtenerTodos()
        {
            var usuarios = await _usuarioService.ObtenerTodosAsync();
            return Ok(usuarios);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> Actualizar(int id, [FromBody] UsuarioActualizarDto dto)
        {
            var usuario = await _usuarioService.ActualizarAsync(id, dto);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });
            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var resultado = await _usuarioService.EliminarAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = "Usuario no encontrado" });
            return Ok(new { mensaje = "Usuario eliminado exitosamente" });
        }
    }
}
