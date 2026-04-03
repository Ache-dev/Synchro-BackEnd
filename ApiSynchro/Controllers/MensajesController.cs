using ApiSynchro.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiSynchro.Controllers
{
    [ApiController]
    [Route("api/v1/mensajes")]
    public class MensajesController : ControllerBase
    {
        private readonly Repository _repository;

        public MensajesController(Repository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<Mensaje>> Crear([FromBody] Mensaje mensaje)
        {
            var id = await _repository.CrearMensajeAsync(mensaje);
            mensaje.IdMensaje = id;
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, mensaje);
        }

        [HttpGet("match/{idMatch}")]
        public async Task<ActionResult<IEnumerable<Mensaje>>> ObtenerPorMatch(int idMatch)
        {
            return Ok(await _repository.ObtenerMensajesPorMatchAsync(idMatch));
        }

        [HttpGet("conversacion/{idRemitente}/{idDestinatario}")]
        public async Task<ActionResult<IEnumerable<Mensaje>>> ObtenerConversacion(int idRemitente, int idDestinatario)
        {
            return Ok(await _repository.ObtenerConversacionAsync(idRemitente, idDestinatario));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mensaje>> ObtenerPorId(int id)
        {
            var mensaje = await _repository.ObtenerMensajePorIdAsync(id);
            return mensaje is null ? NotFound() : Ok(mensaje);
        }

        [HttpGet("no-leidos/{idUsuario}")]
        public async Task<ActionResult<IEnumerable<Mensaje>>> ObtenerNoLeidos(int idUsuario)
        {
            return Ok(await _repository.ObtenerMensajesNoLeidosAsync(idUsuario));
        }

        [HttpPut("{id}/marcar-leido")]
        public async Task<IActionResult> MarcarLeido(int id)
        {
            var actualizado = await _repository.MarcarMensajeComoLeidoAsync(id);
            return actualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _repository.EliminarMensajeAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
    }
}
