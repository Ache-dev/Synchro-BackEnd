using ApiSynchro.DTOs;
using ApiSynchro.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiSynchro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EncuestasController : ControllerBase
    {
        private readonly IEncuestaService _encuestaService;

        public EncuestasController(IEncuestaService encuestaService)
        {
            _encuestaService = encuestaService;
        }

        [HttpGet("preguntas")]
        public async Task<ActionResult<List<PreguntaEncuestaDto>>> ObtenerPreguntas()
        {
            var preguntas = await _encuestaService.ObtenerPreguntasAsync();
            return Ok(preguntas);
        }

        [HttpPost("preguntas")]
        public async Task<ActionResult<PreguntaEncuestaDto>> CrearPregunta([FromBody] PreguntaEncuestaCreateDto dto)
        {
            var pregunta = await _encuestaService.CrearPreguntaAsync(dto);
            return CreatedAtAction(nameof(ObtenerPreguntas), pregunta);
        }

        [HttpPost("respuestas")]
        public async Task<ActionResult<List<RespuestaEncuestaDto>>> GuardarRespuestas(
            [FromQuery] int idUsuario, 
            [FromBody] RespuestasEncuestaCreateDto dto)
        {
            var respuestas = await _encuestaService.GuardarRespuestasAsync(idUsuario, dto);
            return CreatedAtAction(nameof(ObtenerRespuestasPorUsuario), new { idUsuario }, respuestas);
        }

        [HttpGet("respuestas/usuario/{idUsuario}")]
        public async Task<ActionResult<List<RespuestaEncuestaDto>>> ObtenerRespuestasPorUsuario(int idUsuario)
        {
            var respuestas = await _encuestaService.ObtenerRespuestasPorUsuarioAsync(idUsuario);
            return Ok(respuestas);
        }
    }
}
