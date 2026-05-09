using Microsoft.AspNetCore.Mvc;
using Model;
using Query.Interfaces;
using Repository.Interfaces;

namespace ApiSynchro.Controllers
{
    /// <summary>
    /// Controlador para los mensajes
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MensajeController : ControllerBase
    {
        private readonly ILogger<MensajeController> _logger;
        private readonly IMensajeQueries _queries;
        private readonly IMensajeRepository _repository;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="MensajeController"/>
        /// </summary>
        /// <param name="logger">Servicio de logging</param>
        /// <param name="queries">Servicio de consultas de mensajes</param>
        /// <param name="repository">Servicio de persistencia de mensajes</param>
        public MensajeController(ILogger<MensajeController> logger, IMensajeQueries queries, IMensajeRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Lista todos los mensajes
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Mensaje>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Listar()
        {
            var rs = await _queries.GetAll();
            return Ok(rs);
        }

    /// <summary>
    /// Obtiene un mensaje por su identificador
    /// </summary>
    /// <param name="id">Id del mensaje</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Mensaje), 200)]
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
                _logger.LogError(ex, "Error obteniendo mensaje");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Crea un nuevo mensaje
    /// </summary>
    /// <param name="entidad">Datos del mensaje</param>
        [HttpPost]
        [ProducesResponseType(typeof(Mensaje), 201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Crear([FromBody] Mensaje entidad)
        {
            try
            {
                var creada = await _repository.Add(entidad);
                return CreatedAtAction(nameof(PorId), new { id = creada.IdMensaje }, creada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando mensaje");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Actualiza un mensaje existente
    /// </summary>
    /// <param name="id">Id del mensaje</param>
    /// <param name="entidad">Nuevos datos del mensaje</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Mensaje), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Mensaje entidad)
        {
            try
            {
                var existente = await _queries.GetById(id);
                if (existente == null)
                    return NotFound();

                entidad.IdMensaje = id;
                var actualizado = await _repository.Update(entidad);
                return Ok(actualizado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando mensaje");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    /// <summary>
    /// Elimina un mensaje por su identificador
    /// </summary>
    /// <param name="id">Id del mensaje</param>
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
                _logger.LogError(ex, "Error eliminando mensaje");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}