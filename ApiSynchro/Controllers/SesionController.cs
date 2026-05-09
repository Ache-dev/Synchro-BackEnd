using Microsoft.AspNetCore.Mvc;
using Model;
using Query.Interfaces;
using Repository.Interfaces;

namespace ApiSynchro.Controllers
{
    /// <summary>
    /// Controlador para las sesiones de usuario
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SesionController : ControllerBase
    {
        private readonly ILogger<SesionController> _logger;
        private readonly ISesionQueries _queries;
        private readonly ISesionRepository _repository;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="SesionController"/>
        /// </summary>
        /// <param name="logger">Servicio de logging</param>
        /// <param name="queries">Servicio de consultas de sesiones</param>
        /// <param name="repository">Servicio de persistencia de sesiones</param>
        public SesionController(ILogger<SesionController> logger, ISesionQueries queries, ISesionRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Lista todas las sesiones
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Sesion>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Listar()
        {
            var rs = await _queries.GetAll();
            return Ok(rs);
        }

    /// <summary>
    /// Obtiene una sesión por su identificador
    /// </summary>
    /// <param name="id">Id de la sesión</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Sesion), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PorId(int id)
        {
            try
            {
                var entidad = await _queries.GetById(id);
                if (entidad == null)
                    return NotFound();

                return Ok(entidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo sesión");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Crea una nueva sesión
    /// </summary>
    /// <param name="entidad">Datos de la sesión</param>
        [HttpPost]
        [ProducesResponseType(typeof(Sesion), 201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Crear([FromBody] Sesion entidad)
        {
            try
            {
                var creada = await _repository.Add(entidad);
                return CreatedAtAction(nameof(PorId), new { id = creada.IdSesion }, creada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando sesión");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Actualiza una sesión existente
    /// </summary>
    /// <param name="id">Id de la sesión</param>
    /// <param name="entidad">Nuevos datos de la sesión</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Sesion), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Sesion entidad)
        {
            try
            {
                var existente = await _queries.GetById(id);
                if (existente == null)
                    return NotFound();

                entidad.IdSesion = id;
                var actualizado = await _repository.Update(entidad);
                return Ok(actualizado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando sesión");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Elimina una sesión por su identificador
    /// </summary>
    /// <param name="id">Id de la sesión</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var existente = await _queries.GetById(id);
                if (existente == null)
                    return NotFound();

                await _repository.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando sesión");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}