using Microsoft.AspNetCore.Mvc;
using Model;
using Query.Interfaces;
using Repository.Interfaces;

namespace ApiSynchro.Controllers
{
    /// <summary>
    /// Controlador para las respuestas de encuesta
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RespuestaEncuestaController : ControllerBase
    {
        private readonly ILogger<RespuestaEncuestaController> _logger;
        private readonly IRespuestaEncuestaQueries _queries;
        private readonly IRespuestaEncuestaRepository _repository;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="RespuestaEncuestaController"/>
        /// </summary>
        /// <param name="logger">Servicio de logging</param>
        /// <param name="queries">Servicio de consultas de respuestas de encuesta</param>
        /// <param name="repository">Servicio de persistencia de respuestas de encuesta</param>
        public RespuestaEncuestaController(
            ILogger<RespuestaEncuestaController> logger,
            IRespuestaEncuestaQueries queries,
            IRespuestaEncuestaRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Lista todas las respuestas de encuesta
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RespuestaEncuesta>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Listar()
        {
            var rs = await _queries.GetAll();
            return Ok(rs);
        }

    /// <summary>
    /// Obtiene una respuesta de encuesta por su identificador
    /// </summary>
    /// <param name="id">Id de la respuesta</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RespuestaEncuesta), 200)]
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
                _logger.LogError(ex, "Error obteniendo respuesta");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Crea una nueva respuesta de encuesta
    /// </summary>
    /// <param name="entidad">Datos de la respuesta</param>
        [HttpPost]
        [ProducesResponseType(typeof(RespuestaEncuesta), 201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Crear([FromBody] RespuestaEncuesta entidad)
        {
            try
            {
                var creada = await _repository.Add(entidad);
                return CreatedAtAction(nameof(PorId), new { id = creada.IdRespuesta }, creada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando respuesta");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Actualiza una respuesta de encuesta existente
    /// </summary>
    /// <param name="id">Id de la respuesta</param>
    /// <param name="entidad">Nuevos datos de la respuesta</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RespuestaEncuesta), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Actualizar(int id, [FromBody] RespuestaEncuesta entidad)
        {
            try
            {
                var existente = await _queries.GetById(id);
                if (existente == null)
                    return NotFound();

                entidad.IdRespuesta = id;
                var actualizado = await _repository.Update(entidad);
                return Ok(actualizado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando respuesta");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Elimina una respuesta de encuesta por su identificador
    /// </summary>
    /// <param name="id">Id de la respuesta</param>
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
                _logger.LogError(ex, "Error eliminando respuesta");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}