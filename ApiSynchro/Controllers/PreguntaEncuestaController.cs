using Microsoft.AspNetCore.Mvc;
using Model;
using Query.Interfaces;
using Repository.Interfaces;

namespace ApiSynchro.Controllers
{
    /// <summary>
    /// Controlador para las preguntas de encuesta
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PreguntaEncuestaController : ControllerBase
    {
        private readonly ILogger<PreguntaEncuestaController> _logger;
        private readonly IPreguntaEncuestaQueries _queries;
        private readonly IPreguntaEncuestaRepository _repository;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="PreguntaEncuestaController"/>
        /// </summary>
        /// <param name="logger">Servicio de logging</param>
        /// <param name="queries">Servicio de consultas de preguntas de encuesta</param>
        /// <param name="repository">Servicio de persistencia de preguntas de encuesta</param>
        public PreguntaEncuestaController(
            ILogger<PreguntaEncuestaController> logger,
            IPreguntaEncuestaQueries queries,
            IPreguntaEncuestaRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Lista todas las preguntas de encuesta
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PreguntaEncuesta>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Listar()
        {
            var rs = await _queries.GetAll();
            return Ok(rs);
        }

    /// <summary>
    /// Obtiene una pregunta de encuesta por su identificador
    /// </summary>
    /// <param name="id">Id de la pregunta</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PreguntaEncuesta), 200)]
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
                _logger.LogError(ex, "Error obteniendo pregunta");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Crea una nueva pregunta de encuesta
    /// </summary>
    /// <param name="entidad">Datos de la pregunta de encuesta</param>
        [HttpPost]
        [ProducesResponseType(typeof(PreguntaEncuesta), 201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Crear([FromBody] PreguntaEncuesta entidad)
        {
            try
            {
                var creada = await _repository.Add(entidad);
                return CreatedAtAction(nameof(PorId), new { id = creada.IdPregunta }, creada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando pregunta");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Actualiza una pregunta de encuesta existente
    /// </summary>
    /// <param name="id">Id de la pregunta</param>
    /// <param name="entidad">Nuevos datos de la pregunta</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PreguntaEncuesta), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Actualizar(int id, [FromBody] PreguntaEncuesta entidad)
        {
            try
            {
                var existente = await _queries.GetById(id);
                if (existente == null)
                    return NotFound();

                entidad.IdPregunta = id;
                var actualizado = await _repository.Update(entidad);
                return Ok(actualizado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando pregunta");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Elimina una pregunta de encuesta por su identificador
    /// </summary>
    /// <param name="id">Id de la pregunta</param>
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
                _logger.LogError(ex, "Error eliminando pregunta");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}