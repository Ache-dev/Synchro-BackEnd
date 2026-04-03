using ApiSynchro.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiSynchro.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly Repository _repository;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(Repository repository, ILogger<UsuariosController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> ObtenerTodos()
        {
            return Ok(await _repository.ObtenerUsuariosAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> ObtenerPorId(int id)
        {
            var usuario = await _repository.ObtenerUsuarioPorIdAsync(id);
            return usuario is null ? NotFound() : Ok(usuario);
        }

        [HttpPost("registro")]
        public async Task<ActionResult<Usuario>> Registrar([FromBody] Usuario usuario)
        {
            var existente = await _repository.ObtenerUsuarioPorEmailAsync(usuario.Email);
            if (existente is not null)
                return BadRequest(new { mensaje = "El email ya está registrado." });

            var id = await _repository.RegistrarUsuarioAsync(usuario);
            usuario.IdUsuario = id;
            usuario.Contrasena = string.Empty;

            _logger.LogInformation("Usuario registrado: {Email}", usuario.Email);
            var creado = await _repository.ObtenerUsuarioPorIdAsync(id);
            if (creado is not null)
                creado.Contrasena = string.Empty;

            return CreatedAtAction(nameof(ObtenerPorId), new { id }, creado ?? usuario);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiSynchro.DTOs.LoginResponse>> Login([FromBody] ApiSynchro.DTOs.LoginDto loginDto)
        {
            var registrado = await _repository.ObtenerUsuarioPorEmailAsync(loginDto.Email);
            if (registrado is null)
                return Unauthorized(new { mensaje = "Credenciales inválidas." });

            var hashIngresado = Repository.CalcularHash(loginDto.Contrasena);
            if (!string.Equals(registrado.Contrasena, hashIngresado, StringComparison.OrdinalIgnoreCase))
                return Unauthorized(new { mensaje = "Credenciales inválidas." });

            var sesion = new Sesion
            {
                IdUsuario = registrado.IdUsuario,
                Token = Guid.NewGuid().ToString("N"),
                ExpiraEn = DateTime.Now.AddDays(7),
                CreadoEn = DateTime.Now
            };

            sesion.IdSesion = await _repository.CrearSesionAsync(sesion);
            _logger.LogInformation("Login exitoso para usuario: {Email}", registrado.Email);

            // Hide password before returning
            registrado.Contrasena = string.Empty;

            return Ok(new ApiSynchro.DTOs.LoginResponse
            {
                Token = sesion.Token,
                ExpiraEn = sesion.ExpiraEn,
                Usuario = registrado
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromHeader(Name = "Authorization")] string? authorization)
        {
            var token = authorization;
            if (string.IsNullOrWhiteSpace(token))
                return BadRequest(new { mensaje = "Token requerido." });

            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                token = token[7..].Trim();

            var eliminado = await _repository.EliminarSesionPorTokenAsync(token);
            return eliminado ? Ok(new { mensaje = "Sesión cerrada." }) : NotFound(new { mensaje = "Sesión no encontrada." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Usuario usuario)
        {
            var actual = await _repository.ObtenerUsuarioPorIdAsync(id);
            if (actual is null)
                return NotFound();

            usuario.IdUsuario = id;
            var actualizarPassword = !string.IsNullOrWhiteSpace(usuario.Contrasena);
            if (!actualizarPassword)
                usuario.Contrasena = actual.Contrasena;

            var actualizado = await _repository.ActualizarUsuarioAsync(usuario, actualizarPassword);
            return actualizado ? NoContent() : StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _repository.EliminarUsuarioAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
    }
}
