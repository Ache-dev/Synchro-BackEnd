using ApiSynchro.Data;
using ApiSynchro.DTOs;
using ApiSynchro.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiSynchro.Services
{
    public interface IMensajeService
    {
        Task<MensajeDto?> EnviarMensajeAsync(int idRemitente, MensajeCreateDto dto);
        Task<List<MensajeDto>> ObtenerMensajesPorMatchAsync(int idMatch);
        Task<List<MensajeDto>> ObtenerMensajesNoLeidosAsync(int idUsuario);
        Task<bool> MarcarComoLeidoAsync(int idMensaje);
        Task<bool> EliminarMensajeAsync(int idMensaje);
    }

    public class MensajeService : IMensajeService
    {
        private readonly SynchroDbContext _context;

        public MensajeService(SynchroDbContext context)
        {
            _context = context;
        }

        public async Task<MensajeDto?> EnviarMensajeAsync(int idRemitente, MensajeCreateDto dto)
        {
            var match = await _context.Matches.FindAsync(dto.IdMatch);
            if (match == null || !match.Estado)
                return null;

            if (match.IdUsuario1 != idRemitente && match.IdUsuario2 != idRemitente)
                return null;

            var mensaje = new Mensaje
            {
                IdMatch = dto.IdMatch,
                IdRemitente = idRemitente,
                IdDestinatario = dto.IdDestinatario,
                MensajeTexto = dto.MensajeTexto,
                TipoMensaje = dto.TipoMensaje,
                FechaMensaje = DateTime.Now,
                EstadoLeido = false
            };

            _context.Mensajes.Add(mensaje);
            await _context.SaveChangesAsync();

            return await ObtenerMensajePorIdAsync(mensaje.IdMensaje);
        }

        public async Task<List<MensajeDto>> ObtenerMensajesPorMatchAsync(int idMatch)
        {
            var mensajes = await _context.Mensajes
                .Include(m => m.Remitente)
                .Include(m => m.Destinatario)
                .Where(m => m.IdMatch == idMatch)
                .OrderBy(m => m.FechaMensaje)
                .ToListAsync();

            return mensajes.Select(MapearADto).ToList();
        }

        public async Task<List<MensajeDto>> ObtenerMensajesNoLeidosAsync(int idUsuario)
        {
            var mensajes = await _context.Mensajes
                .Include(m => m.Remitente)
                .Include(m => m.Destinatario)
                .Where(m => m.IdDestinatario == idUsuario && !m.EstadoLeido)
                .OrderBy(m => m.FechaMensaje)
                .ToListAsync();

            return mensajes.Select(MapearADto).ToList();
        }

        public async Task<bool> MarcarComoLeidoAsync(int idMensaje)
        {
            var mensaje = await _context.Mensajes.FindAsync(idMensaje);
            if (mensaje == null)
                return false;

            mensaje.EstadoLeido = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarMensajeAsync(int idMensaje)
        {
            var mensaje = await _context.Mensajes.FindAsync(idMensaje);
            if (mensaje == null)
                return false;

            _context.Mensajes.Remove(mensaje);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<MensajeDto?> ObtenerMensajePorIdAsync(int idMensaje)
        {
            var mensaje = await _context.Mensajes
                .Include(m => m.Remitente)
                .Include(m => m.Destinatario)
                .FirstOrDefaultAsync(m => m.IdMensaje == idMensaje);

            return mensaje != null ? MapearADto(mensaje) : null;
        }

        private MensajeDto MapearADto(Mensaje mensaje)
        {
            return new MensajeDto
            {
                IdMensaje = mensaje.IdMensaje,
                IdMatch = mensaje.IdMatch,
                IdRemitente = mensaje.IdRemitente,
                IdDestinatario = mensaje.IdDestinatario,
                MensajeTexto = mensaje.MensajeTexto,
                FechaMensaje = mensaje.FechaMensaje,
                TipoMensaje = mensaje.TipoMensaje,
                EstadoLeido = mensaje.EstadoLeido,
                NombreRemitente = mensaje.Remitente?.Nombre ?? "",
                NombreDestinatario = mensaje.Destinatario?.Nombre ?? ""
            };
        }
    }
}
