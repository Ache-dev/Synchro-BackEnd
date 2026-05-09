using Microsoft.AspNetCore.Mvc;
using Model;
using Query.Interfaces;
using Repository.Interfaces;

namespace ApiSynchro.Controllers
{
    /// <summary>
    /// Controlador para las intenciones de búsqueda
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IntencionBusquedaController : ControllerBase
    {
        private readonly ILogger<IntencionBusquedaController> _logger;
        private readonly IIntencionBusquedaQueries _queries;
        private readonly IIntencionBusquedaRepository _repository;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="IntencionBusquedaController"/>
        /// </summary>
        /// <param name="logger">Servicio de logging</param>
        /// <param name="queries">Servicio de consultas de intenciones de búsqueda</param>
        /// <param name="repository">Servicio de persistencia de intenciones de búsqueda</param>
        public IntencionBusquedaController(
            ILogger<IntencionBusquedaController> logger,
            IIntencionBusquedaQueries queries,
            IIntencionBusquedaRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Lista todas las intenciones de búsqueda
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IntencionBusqueda>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Listar()
        {
            var rs = await _queries.GetAll();
            return Ok(rs);
        }

    /// <summary>
    /// Obtiene una intención de búsqueda por su identificador
    /// </summary>
    /// <param name="id">Id de la intención de búsqueda</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IntencionBusqueda), 200)]
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
                _logger.LogError(ex, "Error obteniendo intención de búsqueda");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Crea una nueva intención de búsqueda
    /// </summary>
    /// <param name="entidad">Datos de la intención de búsqueda</param>
        [HttpPost]
        [ProducesResponseType(typeof(IntencionBusqueda), 201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Crear([FromBody] IntencionBusqueda entidad)
        {
            try
            {
                var creada = await _repository.Add(entidad);
                return CreatedAtAction(nameof(PorId), new { id = creada.IdIntencion }, creada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando intención de búsqueda");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Actualiza una intención de búsqueda existente
    /// </summary>
    /// <param name="id">Id de la intención de búsqueda</param>
    /// <param name="entidad">Nuevos datos de la intención de búsqueda</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IntencionBusqueda), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Actualizar(int id, [FromBody] IntencionBusqueda entidad)
        {
            try
            {
                var existente = await _queries.GetById(id);
                if (existente == null)
                    return NotFound();

                entidad.IdIntencion = id;
                var actualizado = await _repository.Update(entidad);
                return Ok(actualizado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando intención de búsqueda");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Elimina una intención de búsqueda por su identificador
    /// </summary>
    /// <param name="id">Id de la intención de búsqueda</param>
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
                _logger.LogError(ex, "Error eliminando intención de búsqueda");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}