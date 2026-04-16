using ApiSynchro.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiSynchro.Controllers
{
    /// <summary>
    /// Gestiona preguntas y respuestas de encuestas utilizadas para enriquecer perfiles y afinidad.
    /// </summary>
    [ApiController]
    [Route("api/v1/encuestas")]
    public class EncuestasController : ControllerBase
    {
        private readonly IRepository _repository;

        /// <summary>
        /// Inicializa una nueva instancia del controlador de encuestas.
        /// </summary>
        public EncuestasController(IRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtiene todas las preguntas de encuesta ordenadas por posición.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PreguntaEncuesta>>> ObtenerPreguntas()
        {
            return Ok(await _repository.ObtenerPreguntasAsync());
        }

        /// <summary>
        /// Obtiene una pregunta específica por su identificador.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PreguntaEncuesta>> ObtenerPreguntaPorId(int id)
        {
            var pregunta = await _repository.ObtenerPreguntaPorIdAsync(id);
            return pregunta is null ? NotFound() : Ok(pregunta);
        }

        /// <summary>
        /// Crea una nueva pregunta de encuesta.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PreguntaEncuesta>> CrearPregunta([FromBody] PreguntaEncuesta pregunta)
        {
            var id = await _repository.CrearPreguntaAsync(pregunta);
            pregunta.IdPregunta = id;
            return CreatedAtAction(nameof(ObtenerPreguntaPorId), new { id }, pregunta);
        }

        /// <summary>
        /// Actualiza una pregunta existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPregunta(int id, [FromBody] PreguntaEncuesta pregunta)
        {
            pregunta.IdPregunta = id;
            var actualizado = await _repository.ActualizarPreguntaAsync(pregunta);
            return actualizado ? NoContent() : NotFound();
        }

        /// <summary>
        /// Elimina una pregunta de encuesta por su identificador.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPregunta(int id)
        {
            var eliminado = await _repository.EliminarPreguntaAsync(id);
            return eliminado ? NoContent() : NotFound();
        }

        /// <summary>
        /// Obtiene todas las respuestas registradas en encuestas.
        /// </summary>
        [HttpGet("respuestas")]
        public async Task<ActionResult<IEnumerable<RespuestaEncuesta>>> ObtenerRespuestas()
        {
            return Ok(await _repository.ObtenerRespuestasAsync());
        }

        /// <summary>
        /// Obtiene una respuesta de encuesta por identificador.
        /// </summary>
        [HttpGet("respuestas/{id}")]
        public async Task<ActionResult<RespuestaEncuesta>> ObtenerRespuestaPorId(int id)
        {
            var respuesta = await _repository.ObtenerRespuestaPorIdAsync(id);
            return respuesta is null ? NotFound() : Ok(respuesta);
        }

        /// <summary>
        /// Obtiene todas las respuestas asociadas a un usuario.
        /// </summary>
        [HttpGet("usuario/{idUsuario}/respuestas")]
        public async Task<ActionResult<IEnumerable<RespuestaEncuesta>>> ObtenerRespuestasPorUsuario(int idUsuario)
        {
            return Ok(await _repository.ObtenerRespuestasPorUsuarioAsync(idUsuario));
        }

        /// <summary>
        /// Crea una respuesta de encuesta individual.
        /// </summary>
        [HttpPost("respuestas")]
        public async Task<ActionResult<RespuestaEncuesta>> CrearRespuesta([FromBody] RespuestaEncuesta respuesta)
        {
            var id = await _repository.CrearRespuestaAsync(respuesta);
            respuesta.IdRespuesta = id;
            return CreatedAtAction(nameof(ObtenerRespuestaPorId), new { id }, respuesta);
        }

        /// <summary>
        /// Crea múltiples respuestas para un usuario en una sola solicitud.
        /// </summary>
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

        /// <summary>
        /// Actualiza una respuesta de encuesta existente.
        /// </summary>
        [HttpPut("respuestas/{id}")]
        public async Task<IActionResult> ActualizarRespuesta(int id, [FromBody] RespuestaEncuesta respuesta)
        {
            respuesta.IdRespuesta = id;
            var actualizado = await _repository.ActualizarRespuestaAsync(respuesta);
            return actualizado ? NoContent() : NotFound();
        }

        /// <summary>
        /// Elimina una respuesta de encuesta por su identificador.
        /// </summary>
        [HttpDelete("respuestas/{id}")]
        public async Task<IActionResult> EliminarRespuesta(int id)
        {
            var eliminado = await _repository.EliminarRespuestaAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
    }
}
