using ApiSynchro.DTOs;
using ApiSynchro.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiSynchro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpPost]
        public async Task<ActionResult<MatchDto>> CrearMatch([FromQuery] int idUsuario1, [FromBody] MatchCreateDto dto)
        {
            var match = await _matchService.CrearMatchAsync(idUsuario1, dto);
            if (match == null)
                return BadRequest(new { mensaje = "No se pudo crear el match. Verifica que los usuarios existan." });

            return CreatedAtAction(nameof(ObtenerPorId), new { id = match.IdMatch }, match);
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<List<MatchDto>>> ObtenerPorUsuario(int idUsuario)
        {
            var matches = await _matchService.ObtenerMatchesPorUsuarioAsync(idUsuario);
            return Ok(matches);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MatchDto>> ObtenerPorId(int id)
        {
            var match = await _matchService.ObtenerMatchPorIdAsync(id);
            if (match == null)
                return NotFound(new { mensaje = "Match no encontrado" });

            return Ok(match);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MatchDto>> Actualizar(int id, [FromBody] MatchActualizarDto dto)
        {
            var match = await _matchService.ActualizarMatchAsync(id, dto);
            if (match == null)
                return NotFound(new { mensaje = "Match no encontrado" });

            return Ok(match);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var resultado = await _matchService.EliminarMatchAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = "Match no encontrado" });

            return Ok(new { mensaje = "Match eliminado exitosamente" });
        }
    }
}
