using ApiSynchro.DTOs;
using ApiSynchro.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiSynchro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MensajesController : ControllerBase
    {
        private readonly IMensajeService _mensajeService;

        public MensajesController(IMensajeService mensajeService)
        {
            _mensajeService = mensajeService;
        }

        [HttpPost]
        public async Task<ActionResult<MensajeDto>> EnviarMensaje([FromQuery] int idRemitente, [FromBody] MensajeCreateDto dto)
        {
            var mensaje = await _mensajeService.EnviarMensajeAsync(idRemitente, dto);
            if (mensaje == null)
                return BadRequest(new { mensaje = "No se pudo enviar el mensaje. Verifica el match y los usuarios." });

            return CreatedAtAction(nameof(ObtenerPorMatch), new { idMatch = dto.IdMatch }, mensaje);
        }

        [HttpGet("match/{idMatch}")]
        public async Task<ActionResult<List<MensajeDto>>> ObtenerPorMatch(int idMatch)
        {
            var mensajes = await _mensajeService.ObtenerMensajesPorMatchAsync(idMatch);
            return Ok(mensajes);
        }

        [HttpGet("no-leidos/{idUsuario}")]
        public async Task<ActionResult<List<MensajeDto>>> ObtenerNoLeidos(int idUsuario)
        {
            var mensajes = await _mensajeService.ObtenerMensajesNoLeidosAsync(idUsuario);
            return Ok(mensajes);
        }

        [HttpPut("{id}/leer")]
        public async Task<ActionResult> MarcarComoLeido(int id)
        {
            var resultado = await _mensajeService.MarcarComoLeidoAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = "Mensaje no encontrado" });

            return Ok(new { mensaje = "Mensaje marcado como leído" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var resultado = await _mensajeService.EliminarMensajeAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = "Mensaje no encontrado" });

            return Ok(new { mensaje = "Mensaje eliminado exitosamente" });
        }
    }
}
