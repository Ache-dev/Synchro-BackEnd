using ApiSynchro.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiSynchro.Controllers
{
    [ApiController]
    [Route("api/v1/matches")]
    public class MatchesController : ControllerBase
    {
        private readonly Repository _repository;

        public MatchesController(Repository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> ObtenerTodos()
        {
            return Ok(await _repository.ObtenerMatchesAsync());
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<IEnumerable<Match>>> ObtenerPorUsuario(int idUsuario)
        {
            return Ok(await _repository.ObtenerMatchesPorUsuarioAsync(idUsuario));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> ObtenerPorId(int id)
        {
            var match = await _repository.ObtenerMatchPorIdAsync(id);
            return match is null ? NotFound() : Ok(match);
        }

        [HttpPost]
        public async Task<ActionResult<Match>> Crear([FromBody] Match match)
        {
            var id = await _repository.CrearMatchAsync(match);
            match.IdMatch = id;
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, match);
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromQuery] bool estado)
        {
            var actualizado = await _repository.ActualizarEstadoMatchAsync(id, estado);
            return actualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _repository.EliminarMatchAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
    }
}
