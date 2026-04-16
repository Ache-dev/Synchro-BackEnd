using ApiSynchro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ApiSynchro.Controllers
{
    /// <summary>
    /// Administra operaciones de consulta, creación, sincronización y cierre de matches entre usuarios.
    /// </summary>
    [ApiController]
    [Route("api/v1/matches")]
    public class MatchesController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IHubContext<ApiSynchro.Hubs.ChatHub> _hubContext;

        /// <summary>
        /// Inicializa una nueva instancia del controlador de matches.
        /// </summary>
        public MatchesController(IRepository repository, IHubContext<ApiSynchro.Hubs.ChatHub> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Obtiene todos los matches registrados.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> ObtenerTodos()
        {
            return Ok(await _repository.ObtenerMatchesAsync());
        }

        /// <summary>
        /// Obtiene los matches de un usuario específico.
        /// </summary>
        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<IEnumerable<Match>>> ObtenerPorUsuario(int idUsuario)
        {
            return Ok(await _repository.ObtenerMatchesPorUsuarioAsync(idUsuario));
        }

        /// <summary>
        /// Obtiene un match por su identificador.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> ObtenerPorId(int id)
        {
            var match = await _repository.ObtenerMatchPorIdAsync(id);
            return match is null ? NotFound() : Ok(match);
        }

        /// <summary>
        /// Crea un nuevo registro de match.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Match>> Crear([FromBody] Match match)
        {
            var id = await _repository.CrearMatchAsync(match);
            match.IdMatch = id;
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, match);
        }

        /// <summary>
        /// Actualiza el estado de un match existente.
        /// </summary>
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromQuery] bool estado)
        {
            var actualizado = await _repository.ActualizarEstadoMatchAsync(id, estado);
            return actualizado ? NoContent() : NotFound();
        }

        /// <summary>
        /// Ejecuta la sincronización automática para encontrar el mejor match por afinidad.
        /// </summary>
        [HttpPost("sincronizar/{idUsuario}")]
        public async Task<ActionResult<Match>> Sincronizar(int idUsuario)
        {
            var match = await _repository.SincronizarMatchAsync(idUsuario);
            return match == null ? NotFound(new { mensaje = "No se encontró un match con afinidad suficiente en este momento." }) : Ok(match);
        }

        /// <summary>
        /// Elimina un match y notifica a los clientes conectados por SignalR.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var match = await _repository.ObtenerMatchPorIdAsync(id);
            if (match == null)
            {
                return NotFound();
            }

            var eliminado = await _repository.EliminarMatchAsync(id);
            if (eliminado)
            {
                await _hubContext.Clients.All.SendAsync("MatchCerrado", id);
            }

            return eliminado ? NoContent() : NotFound();
        }
    }
}
