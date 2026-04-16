using ApiSynchro.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiSynchro.Controllers
{
    /// <summary>
    /// Expone endpoints CRUD para administrar las intenciones de búsqueda de los usuarios.
    /// </summary>
    [ApiController]
    [Route("api/v1/intenciones")]
    public class IntencionesController : ControllerBase
    {
        private readonly IRepository _repository;

        /// <summary>
        /// Inicializa una nueva instancia del controlador de intenciones.
        /// </summary>
        public IntencionesController(IRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtiene todas las intenciones configuradas.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IntencionBusqueda>>> ObtenerTodas()
        {
            return Ok(await _repository.ObtenerIntencionesAsync());
        }

        /// <summary>
        /// Obtiene una intención por su identificador.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<IntencionBusqueda>> ObtenerPorId(int id)
        {
            var intencion = await _repository.ObtenerIntencionPorIdAsync(id);
            return intencion is null ? NotFound() : Ok(intencion);
        }

        /// <summary>
        /// Crea una nueva intención de búsqueda.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<IntencionBusqueda>> Crear([FromBody] IntencionBusqueda intencion)
        {
            var id = await _repository.CrearIntencionAsync(intencion);
            intencion.IdIntencion = id;
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, intencion);
        }

        /// <summary>
        /// Actualiza una intención existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] IntencionBusqueda intencion)
        {
            intencion.IdIntencion = id;
            var actualizado = await _repository.ActualizarIntencionAsync(intencion);
            return actualizado ? NoContent() : NotFound();
        }

        /// <summary>
        /// Elimina una intención por su identificador.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _repository.EliminarIntencionAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
    }
}
