using ApiSynchro.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiSynchro.Controllers
{
    /// <summary>
    /// Gestiona el ciclo de vida de usuarios, autenticación por sesión y actualización de perfil.
    /// </summary>
    [ApiController]
    [Route("api/v1/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<UsuariosController> _logger;

        /// <summary>
        /// Inicializa una nueva instancia del controlador de usuarios.
        /// </summary>
        public UsuariosController(IRepository repository, ILogger<UsuariosController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los usuarios registrados.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> ObtenerTodos()
        {
            return Ok(await _repository.ObtenerUsuariosAsync());
        }

        /// <summary>
        /// Obtiene un usuario por su identificador.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> ObtenerPorId(int id)
        {
            var usuario = await _repository.ObtenerUsuarioPorIdAsync(id);
            return usuario is null ? NotFound() : Ok(usuario);
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        [HttpPost("registro")]
        public async Task<ActionResult<Usuario>> Registrar([FromBody] Usuario usuario)
        {
            var existente = await _repository.ObtenerUsuarioPorEmailAsync(usuario.Email);
            if (existente is not null)
            {
                return BadRequest(new { mensaje = "El email ya está registrado." });
            }

            var id = await _repository.RegistrarUsuarioAsync(usuario);
            usuario.IdUsuario = id;
            usuario.Contrasena = string.Empty;

            _logger.LogInformation("Usuario registrado: {Email}", usuario.Email);
            var creado = await _repository.ObtenerUsuarioPorIdAsync(id);
            if (creado is not null)
            {
                creado.Contrasena = string.Empty;
            }

            return CreatedAtAction(nameof(ObtenerPorId), new { id }, creado ?? usuario);
        }

        /// <summary>
        /// Inicia sesión validando credenciales y creando un token de sesión persistido.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Dictionary<string, string> credenciales)
        {
            if (!credenciales.TryGetValue("email", out var email) || string.IsNullOrWhiteSpace(email) ||
                !credenciales.TryGetValue("contrasena", out var contrasena) || string.IsNullOrWhiteSpace(contrasena))
            {
                return BadRequest(new { mensaje = "Email y contraseña son requeridos." });
            }

            var registrado = await _repository.ObtenerUsuarioPorEmailAsync(email);
            if (registrado is null)
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas." });
            }

            var hashIngresado = _repository.CalcularHash(contrasena);
            if (!string.Equals(registrado.Contrasena, hashIngresado, StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas." });
            }

            var sesion = new Sesion
            {
                IdUsuario = registrado.IdUsuario,
                Token = Guid.NewGuid().ToString("N"),
                ExpiraEn = DateTime.Now.AddDays(7),
                CreadoEn = DateTime.Now
            };

            sesion.IdSesion = await _repository.CrearSesionAsync(sesion);

            Response.Cookies.Append("synchro_session", sesion.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = sesion.ExpiraEn
            });

            _logger.LogInformation("Login exitoso para usuario: {Email}", registrado.Email);
            registrado.Contrasena = string.Empty;

            return Ok(new
            {
                token = sesion.Token,
                expiraEn = sesion.ExpiraEn,
                usuario = registrado
            });
        }

        /// <summary>
        /// Cierra la sesión activa eliminando el token enviado por cabecera o cookie.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromHeader(Name = "Authorization")] string? authorization)
        {
            var token = authorization;

            if (string.IsNullOrWhiteSpace(token) && Request.Cookies.TryGetValue("synchro_session", out var cookieToken))
            {
                token = cookieToken;
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest(new { mensaje = "Token requerido." });
            }

            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token[7..].Trim();
            }

            var eliminado = await _repository.EliminarSesionPorTokenAsync(token);
            Response.Cookies.Delete("synchro_session");

            return eliminado ? Ok(new { mensaje = "Sesión cerrada." }) : NotFound(new { mensaje = "Sesión no encontrada." });
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Usuario usuario)
        {
            var actual = await _repository.ObtenerUsuarioPorIdAsync(id);
            if (actual is null)
            {
                return NotFound();
            }

            usuario.IdUsuario = id;
            var actualizarPassword = !string.IsNullOrWhiteSpace(usuario.Contrasena);
            if (!actualizarPassword)
            {
                usuario.Contrasena = actual.Contrasena;
            }

            var actualizado = await _repository.ActualizarUsuarioAsync(usuario, actualizarPassword);
            return actualizado ? NoContent() : StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Actualiza el vector de embedding del perfil de un usuario.
        /// </summary>
        [HttpPut("{id}/embedding")]
        public async Task<IActionResult> ActualizarEmbedding(int id, [FromBody] Dictionary<string, string> body)
        {
            if (!body.TryGetValue("embedding", out var embedding) || string.IsNullOrWhiteSpace(embedding))
            {
                return BadRequest(new { mensaje = "Embedding es requerido." });
            }

            var actualizado = await _repository.ActualizarEmbeddingUsuarioAsync(id, embedding);
            return actualizado ? Ok(new { mensaje = "Embedding actualizado." }) : NotFound();
        }

        /// <summary>
        /// Elimina un usuario del sistema.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _repository.EliminarUsuarioAsync(id);
            return eliminado ? NoContent() : NotFound();
        }

        /// <summary>
        /// Genera una biografía sugerida para el usuario y la persiste en su perfil.
        /// </summary>
        [HttpPost("{id}/generar-bio")]
        public async Task<ActionResult> GenerarBio(int id)
        {
            var usuario = await _repository.ObtenerUsuarioPorIdAsync(id);
            if (usuario is null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            usuario.BioAI = string.IsNullOrWhiteSpace(usuario.Ciudad)
                ? $"{usuario.Nombre} está abierto a conocer nuevas personas y compartir buenos momentos."
                : $"{usuario.Nombre} vive en {usuario.Ciudad} y está abierto a conocer nuevas personas con intereses afines.";

            var actualizado = await _repository.ActualizarUsuarioAsync(usuario, actualizarPassword: false);
            if (!actualizado)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "No se pudo actualizar la bio." });
            }

            _logger.LogInformation("Bio generada para usuario: {IdUsuario}", id);
            return Ok(new { bioAI = usuario.BioAI });
        }
    }
}
