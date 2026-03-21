using ApiSynchro.Data;
using ApiSynchro.DTOs;
using ApiSynchro.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiSynchro.Services
{
    public interface IEncuestaService
    {
        Task<List<PreguntaEncuestaDto>> ObtenerPreguntasAsync();
        Task<PreguntaEncuestaDto?> CrearPreguntaAsync(PreguntaEncuestaCreateDto dto);
        Task<List<RespuestaEncuestaDto>> GuardarRespuestasAsync(int idUsuario, RespuestasEncuestaCreateDto dto);
        Task<List<RespuestaEncuestaDto>> ObtenerRespuestasPorUsuarioAsync(int idUsuario);
    }

    public class EncuestaService : IEncuestaService
    {
        private readonly SynchroDbContext _context;

        public EncuestaService(SynchroDbContext context)
        {
            _context = context;
        }

        public async Task<List<PreguntaEncuestaDto>> ObtenerPreguntasAsync()
        {
            var preguntas = await _context.PreguntasEncuesta
                .OrderBy(p => p.Orden)
                .ToListAsync();

            return preguntas.Select(p => new PreguntaEncuestaDto
            {
                IdPregunta = p.IdPregunta,
                TextoPregunta = p.TextoPregunta,
                TextoPreguntaEN = p.TextoPreguntaEN,
                Icono = p.Icono,
                Orden = p.Orden
            }).ToList();
        }

        public async Task<PreguntaEncuestaDto?> CrearPreguntaAsync(PreguntaEncuestaCreateDto dto)
        {
            var pregunta = new PreguntaEncuesta
            {
                TextoPregunta = dto.TextoPregunta,
                TextoPreguntaEN = dto.TextoPreguntaEN,
                Icono = dto.Icono,
                Orden = dto.Orden
            };

            _context.PreguntasEncuesta.Add(pregunta);
            await _context.SaveChangesAsync();

            return new PreguntaEncuestaDto
            {
                IdPregunta = pregunta.IdPregunta,
                TextoPregunta = pregunta.TextoPregunta,
                TextoPreguntaEN = pregunta.TextoPreguntaEN,
                Icono = pregunta.Icono,
                Orden = pregunta.Orden
            };
        }

        public async Task<List<RespuestaEncuestaDto>> GuardarRespuestasAsync(int idUsuario, RespuestasEncuestaCreateDto dto)
        {
            var respuestas = new List<RespuestaEncuesta>();

            foreach (var respuestaDto in dto.Respuestas)
            {
                var respuesta = new RespuestaEncuesta
                {
                    IdUsuario = idUsuario,
                    IdPregunta = respuestaDto.IdPregunta,
                    RespuestaTexto = respuestaDto.RespuestaTexto
                };

                respuestas.Add(respuesta);
            }

            _context.RespuestasEncuesta.AddRange(respuestas);
            await _context.SaveChangesAsync();

            return respuestas.Select(r => new RespuestaEncuestaDto
            {
                IdRespuesta = r.IdRespuesta,
                IdUsuario = r.IdUsuario,
                IdPregunta = r.IdPregunta,
                RespuestaTexto = r.RespuestaTexto
            }).ToList();
        }

        public async Task<List<RespuestaEncuestaDto>> ObtenerRespuestasPorUsuarioAsync(int idUsuario)
        {
            var respuestas = await _context.RespuestasEncuesta
                .Where(r => r.IdUsuario == idUsuario)
                .ToListAsync();

            return respuestas.Select(r => new RespuestaEncuestaDto
            {
                IdRespuesta = r.IdRespuesta,
                IdUsuario = r.IdUsuario,
                IdPregunta = r.IdPregunta,
                RespuestaTexto = r.RespuestaTexto
            }).ToList();
        }
    }
}
