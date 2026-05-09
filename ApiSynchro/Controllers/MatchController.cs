using Microsoft.AspNetCore.Mvc;
using Model;
using Query.Interfaces;
using Repository.Interfaces;

namespace ApiSynchro.Controllers
{
    /// <summary>
    /// Controlador para los matches
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly ILogger<MatchController> _logger;
        private readonly IMatchQueries _queries;
        private readonly IMatchRepository _repository;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="MatchController"/>
        /// </summary>
        /// <param name="logger">Servicio de logging</param>
        /// <param name="queries">Servicio de consultas de matches</param>
        /// <param name="repository">Servicio de persistencia de matches</param>
        public MatchController(ILogger<MatchController> logger, IMatchQueries queries, IMatchRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Lista todos los matches
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Match>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Listar()
        {
            var rs = await _queries.GetAll();
            return Ok(rs);
        }

    /// <summary>
    /// Obtiene un match por su identificador
    /// </summary>
    /// <param name="id">Id del match</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Match), 200)]
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
                _logger.LogError(ex, "Error obteniendo match");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Crea un nuevo match
    /// </summary>
    /// <param name="entidad">Datos del match</param>
        [HttpPost]
        [ProducesResponseType(typeof(Match), 201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Crear([FromBody] Match entidad)
        {
            try
            {
                var creada = await _repository.Add(entidad);
                return CreatedAtAction(nameof(PorId), new { id = creada.IdMatch }, creada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando match");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Actualiza un match existente
    /// </summary>
    /// <param name="id">Id del match</param>
    /// <param name="entidad">Nuevos datos del match</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Match), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Match entidad)
        {
            try
            {
                var existente = await _queries.GetById(id);
                if (existente == null)
                    return NotFound();

                entidad.IdMatch = id;
                var actualizado = await _repository.Update(entidad);
                return Ok(actualizado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando match");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Elimina un match por su identificador
    /// </summary>
    /// <param name="id">Id del match</param>
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
                _logger.LogError(ex, "Error eliminando match");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}