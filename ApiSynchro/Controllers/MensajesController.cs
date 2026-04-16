using ApiSynchro.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiSynchro.Controllers
{
    /// <summary>
    /// Gestiona el intercambio de mensajes entre usuarios y consultas de historial de chat.
    /// </summary>
    [ApiController]
    [Route("api/v1/mensajes")]
    public class MensajesController : ControllerBase
    {
        private readonly IRepository _repository;

        /// <summary>
        /// Inicializa una nueva instancia del controlador de mensajes.
        /// </summary>
        public MensajesController(IRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Crea un nuevo mensaje.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Mensaje>> Crear([FromBody] Mensaje mensaje)
        {
            var id = await _repository.CrearMensajeAsync(mensaje);
            mensaje.IdMensaje = id;
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, mensaje);
        }

        /// <summary>
        /// Obtiene todos los mensajes de un match.
        /// </summary>
        [HttpGet("match/{idMatch}")]
        public async Task<ActionResult<IEnumerable<Mensaje>>> ObtenerPorMatch(int idMatch)
        {
            return Ok(await _repository.ObtenerMensajesPorMatchAsync(idMatch));
        }

        /// <summary>
        /// Obtiene la conversación entre dos usuarios.
        /// </summary>
        [HttpGet("conversacion/{idRemitente}/{idDestinatario}")]
        public async Task<ActionResult<IEnumerable<Mensaje>>> ObtenerConversacion(int idRemitente, int idDestinatario)
        {
            return Ok(await _repository.ObtenerConversacionAsync(idRemitente, idDestinatario));
        }

        /// <summary>
        /// Obtiene un mensaje por su identificador.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Mensaje>> ObtenerPorId(int id)
        {
            var mensaje = await _repository.ObtenerMensajePorIdAsync(id);
            return mensaje is null ? NotFound() : Ok(mensaje);
        }

        /// <summary>
        /// Obtiene los mensajes no leídos de un usuario.
        /// </summary>
        [HttpGet("no-leidos/{idUsuario}")]
        public async Task<ActionResult<IEnumerable<Mensaje>>> ObtenerNoLeidos(int idUsuario)
        {
            return Ok(await _repository.ObtenerMensajesNoLeidosAsync(idUsuario));
        }

        /// <summary>
        /// Marca como leído un mensaje específico.
        /// </summary>
        [HttpPut("{id}/marcar-leido")]
        public async Task<IActionResult> MarcarLeido(int id)
        {
            var actualizado = await _repository.MarcarMensajeComoLeidoAsync(id);
            return actualizado ? NoContent() : NotFound();
        }

        /// <summary>
        /// Elimina un mensaje por su identificador.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _repository.EliminarMensajeAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
    }
}
