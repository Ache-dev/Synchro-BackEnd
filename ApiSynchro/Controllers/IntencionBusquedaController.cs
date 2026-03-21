using ApiSynchro.DTOs;
using ApiSynchro.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiSynchro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IntencionBusquedaController : ControllerBase
    {
        private readonly IIntencionBusquedaService _intencionService;

        public IntencionBusquedaController(IIntencionBusquedaService intencionService)
        {
            _intencionService = intencionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<IntencionBusquedaResponseDto>>> ObtenerTodas()
        {
            var intenciones = await _intencionService.ObtenerTodasAsync();
            return Ok(intenciones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IntencionBusquedaResponseDto>> ObtenerPorId(int id)
        {
            var intencion = await _intencionService.ObtenerPorIdAsync(id);
            if (intencion == null)
                return NotFound(new { mensaje = "Intención de búsqueda no encontrada" });

            return Ok(intencion);
        }

        [HttpPost]
        public async Task<ActionResult<IntencionBusquedaResponseDto>> Crear([FromBody] IntencionBusquedaCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensaje = "Datos inválidos", errores = ModelState });

            var intencion = await _intencionService.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = intencion.IdIntencion }, intencion);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IntencionBusquedaResponseDto>> Actualizar(int id, [FromBody] IntencionBusquedaUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensaje = "Datos inválidos", errores = ModelState });

            var intencion = await _intencionService.ActualizarAsync(id, dto);
            if (intencion == null)
                return NotFound(new { mensaje = "Intención de búsqueda no encontrada" });

            return Ok(intencion);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var resultado = await _intencionService.EliminarAsync(id);
            if (!resultado)
                return BadRequest(new { mensaje = "No se puede eliminar la intención de búsqueda. Puede estar en uso o no existe." });

            return Ok(new { mensaje = "Intención de búsqueda eliminada exitosamente" });
        }
    }
}
