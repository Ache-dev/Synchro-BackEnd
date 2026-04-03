using ApiSynchro.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiSynchro.Controllers
{
    [ApiController]
    [Route("api/v1/intenciones")]
    public class IntencionesController : ControllerBase
    {
        private readonly Repository _repository;

        public IntencionesController(Repository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IntencionBusqueda>>> ObtenerTodas()
        {
            return Ok(await _repository.ObtenerIntencionesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IntencionBusqueda>> ObtenerPorId(int id)
        {
            var intencion = await _repository.ObtenerIntencionPorIdAsync(id);
            return intencion is null ? NotFound() : Ok(intencion);
        }

        [HttpPost]
        public async Task<ActionResult<IntencionBusqueda>> Crear([FromBody] IntencionBusqueda intencion)
        {
            var id = await _repository.CrearIntencionAsync(intencion);
            intencion.IdIntencion = id;
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, intencion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] IntencionBusqueda intencion)
        {
            intencion.IdIntencion = id;
            var actualizado = await _repository.ActualizarIntencionAsync(intencion);
            return actualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _repository.EliminarIntencionAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
    }
}
