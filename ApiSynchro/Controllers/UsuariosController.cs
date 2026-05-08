using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Model;
using Query.Interfaces;
using Repository.Interfaces;

namespace ApiSynchro.Controllers
{
    /// <sumary>
    /// Controlador para los usuarios
    /// </sumary>

    [Route("api/[controller]")]
    [ApiController]

    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioQueries _usuarioQueries;
        private readonly IUsuarioRepository _usuarioRepository;
        
        /// <summary>
        /// UsuarioController
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="usuarioQueries"></param>
        /// <param name="usuarioRepository"></param>
        public UsuarioController(
            ILogger<UsuarioController> logger,
            IUsuarioQueries usuarioQueries,
            IUsuarioRepository usuarioRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _usuarioQueries = usuarioQueries ?? throw new ArgumentNullException(nameof(usuarioQueries));
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
        }
        
        /// <summary>
        /// Lista todos los usuarios
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Usuario>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ListarUsuarios()
        {
            _logger.LogInformation("Iniciando listado de todos los usuarios");
            var rs = await _usuarioQueries.GetAll();
            return Ok(rs);
        }

        /// <summary>
        /// Busca un usuario por id y nombre
        /// </summary>
        /// <param name="id">Id del usuario</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Usuario), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UsuarioId(int id)
        {
            try
            {
                var us = await _usuarioQueries.GetById(id);
                if (us == null)
                    return NotFound();

                return Ok(us);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Texto cristian del error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Usuario), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CrearUsuario([FromBody] Usuario usuario)
        {
            try
            {
                var existente = await _usuarioQueries.GetByEmail(usuario.Email);
                if (existente != null)
                    return BadRequest(new { mensaje = "El email ya existe" });

                var creado = await _usuarioRepository.Add(usuario);
                return CreatedAtAction(nameof(UsuarioId), new { id = creado.IdUsuario }, creado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando usuario");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Usuario), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] Usuario usuario)
        {
            try
            {
                var existente = await _usuarioQueries.GetById(id);
                if (existente == null)
                    return NotFound();

                usuario.IdUsuario = id;
                var actualizado = await _usuarioRepository.Update(usuario);
                return Ok(actualizado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando usuario");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Elimina un usuario por id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            try
            {
                var existente = await _usuarioQueries.GetById(id);
                if (existente == null)
                    return NotFound();

                await _usuarioRepository.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando usuario");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}