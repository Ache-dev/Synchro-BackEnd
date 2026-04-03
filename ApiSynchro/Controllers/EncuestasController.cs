using ApiSynchro.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiSynchro.Controllers
{
    [ApiController]
    [Route("api/v1/encuestas")]
    public class EncuestasController : ControllerBase
    {
        private readonly Repository _repository;

        public EncuestasController(Repository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PreguntaEncuesta>>> ObtenerPreguntas()
        {
            return Ok(await _repository.ObtenerPreguntasAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PreguntaEncuesta>> ObtenerPreguntaPorId(int id)
        {
            var pregunta = await _repository.ObtenerPreguntaPorIdAsync(id);
            return pregunta is null ? NotFound() : Ok(pregunta);
        }

        [HttpPost]
        public async Task<ActionResult<PreguntaEncuesta>> CrearPregunta([FromBody] PreguntaEncuesta pregunta)
        {
            var id = await _repository.CrearPreguntaAsync(pregunta);
            pregunta.IdPregunta = id;
            return CreatedAtAction(nameof(ObtenerPreguntaPorId), new { id }, pregunta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPregunta(int id, [FromBody] PreguntaEncuesta pregunta)
        {
            pregunta.IdPregunta = id;
            var actualizado = await _repository.ActualizarPreguntaAsync(pregunta);
            return actualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPregunta(int id)
        {
            var eliminado = await _repository.EliminarPreguntaAsync(id);
            return eliminado ? NoContent() : NotFound();
        }

        [HttpGet("respuestas")]
        public async Task<ActionResult<IEnumerable<RespuestaEncuesta>>> ObtenerRespuestas()
        {
            return Ok(await _repository.ObtenerRespuestasAsync());
        }

        [HttpGet("respuestas/{id}")]
        public async Task<ActionResult<RespuestaEncuesta>> ObtenerRespuestaPorId(int id)
        {
            var respuesta = await _repository.ObtenerRespuestaPorIdAsync(id);
            return respuesta is null ? NotFound() : Ok(respuesta);
        }

        [HttpGet("usuario/{idUsuario}/respuestas")]
        public async Task<ActionResult<IEnumerable<RespuestaEncuesta>>> ObtenerRespuestasPorUsuario(int idUsuario)
        {
            return Ok(await _repository.ObtenerRespuestasPorUsuarioAsync(idUsuario));
        }

        [HttpPost("respuestas")]
        public async Task<ActionResult<RespuestaEncuesta>> CrearRespuesta([FromBody] RespuestaEncuesta respuesta)
        {
            var id = await _repository.CrearRespuestaAsync(respuesta);
            respuesta.IdRespuesta = id;
            return CreatedAtAction(nameof(ObtenerRespuestaPorId), new { id }, respuesta);
        }

        [HttpPost("respuestas/batch")]
        public async Task<IActionResult> CrearRespuestasBatch([FromQuery] int idUsuario, [FromBody] IEnumerable<RespuestaEncuesta> respuestas)
        {
            foreach (var r in respuestas)
            {
                r.IdUsuario = idUsuario;
                await _repository.CrearRespuestaAsync(r);
            }
            return Ok(new { mensaje = "Respuestas guardadas correctamente." });
        }

        [HttpPut("respuestas/{id}")]
        public async Task<IActionResult> ActualizarRespuesta(int id, [FromBody] RespuestaEncuesta respuesta)
        {
            respuesta.IdRespuesta = id;
            var actualizado = await _repository.ActualizarRespuestaAsync(respuesta);
            return actualizado ? NoContent() : NotFound();
        }

        [HttpDelete("respuestas/{id}")]
        public async Task<IActionResult> EliminarRespuesta(int id)
        {
            var eliminado = await _repository.EliminarRespuestaAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
    }
}
