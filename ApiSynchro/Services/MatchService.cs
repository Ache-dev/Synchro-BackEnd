using ApiSynchro.Data;
using ApiSynchro.DTOs;
using ApiSynchro.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiSynchro.Services
{
    public interface IMatchService
    {
        Task<MatchDto?> CrearMatchAsync(int idUsuario1, MatchCreateDto dto);
        Task<List<MatchDto>> ObtenerMatchesPorUsuarioAsync(int idUsuario);
        Task<MatchDto?> ObtenerMatchPorIdAsync(int idMatch);
        Task<MatchDto?> ActualizarMatchAsync(int idMatch, MatchActualizarDto dto);
        Task<bool> EliminarMatchAsync(int idMatch);
    }

    public class MatchService : IMatchService
    {
        private readonly SynchroDbContext _context;

        public MatchService(SynchroDbContext context)
        {
            _context = context;
        }

        public async Task<MatchDto?> CrearMatchAsync(int idUsuario1, MatchCreateDto dto)
        {
            var usuario1 = await _context.Usuarios.FindAsync(idUsuario1);
            var usuario2 = await _context.Usuarios.FindAsync(dto.IdUsuario2);

            if (usuario1 == null || usuario2 == null)
                return null;

            var match = new Match
            {
                IdUsuario1 = idUsuario1,
                IdUsuario2 = dto.IdUsuario2,
                Compatibilidad = dto.Compatibilidad,
                ExplicacionAfinidad = dto.ExplicacionAfinidad,
                SugerenciaIA = dto.SugerenciaIA,
                FechaMatch = DateTime.Now,
                Estado = true
            };

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            return await ObtenerMatchPorIdAsync(match.IdMatch);
        }

        public async Task<List<MatchDto>> ObtenerMatchesPorUsuarioAsync(int idUsuario)
        {
            var matches = await _context.Matches
                .Include(m => m.Usuario1)
                .Include(m => m.Usuario2)
                .Where(m => (m.IdUsuario1 == idUsuario || m.IdUsuario2 == idUsuario) && m.Estado)
                .ToListAsync();

            return matches.Select(MapearADto).ToList();
        }

        public async Task<MatchDto?> ObtenerMatchPorIdAsync(int idMatch)
        {
            var match = await _context.Matches
                .Include(m => m.Usuario1)
                .Include(m => m.Usuario2)
                .FirstOrDefaultAsync(m => m.IdMatch == idMatch);

            return match != null ? MapearADto(match) : null;
        }

        public async Task<MatchDto?> ActualizarMatchAsync(int idMatch, MatchActualizarDto dto)
        {
            var match = await _context.Matches.FindAsync(idMatch);
            if (match == null)
                return null;

            if (dto.Compatibilidad.HasValue)
                match.Compatibilidad = dto.Compatibilidad;
            if (dto.ExplicacionAfinidad != null)
                match.ExplicacionAfinidad = dto.ExplicacionAfinidad;
            if (dto.SugerenciaIA != null)
                match.SugerenciaIA = dto.SugerenciaIA;
            if (dto.Estado.HasValue)
                match.Estado = dto.Estado.Value;

            match.FechaActualizacion = DateTime.Now;
            await _context.SaveChangesAsync();

            return await ObtenerMatchPorIdAsync(idMatch);
        }

        public async Task<bool> EliminarMatchAsync(int idMatch)
        {
            var match = await _context.Matches.FindAsync(idMatch);
            if (match == null)
                return false;

            match.Estado = false;
            await _context.SaveChangesAsync();
            return true;
        }

        private MatchDto MapearADto(Match match)
        {
            return new MatchDto
            {
                IdMatch = match.IdMatch,
                IdUsuario1 = match.IdUsuario1,
                IdUsuario2 = match.IdUsuario2,
                Compatibilidad = match.Compatibilidad,
                ExplicacionAfinidad = match.ExplicacionAfinidad,
                FechaMatch = match.FechaMatch,
                FechaActualizacion = match.FechaActualizacion,
                SugerenciaIA = match.SugerenciaIA,
                Estado = match.Estado,
                Usuario1 = match.Usuario1 != null ? MapearUsuarioADto(match.Usuario1) : null,
                Usuario2 = match.Usuario2 != null ? MapearUsuarioADto(match.Usuario2) : null
            };
        }

        private UsuarioResponseDto MapearUsuarioADto(Usuario usuario)
        {
            return new UsuarioResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                FechaNacimiento = usuario.FechaNacimiento,
                Ciudad = usuario.Ciudad,
                IntencionBusqueda = usuario.IntencionBusqueda,
                Genero = usuario.Genero,
                FotoPerfil = usuario.FotoPerfil,
                IdiomaPreferido = usuario.IdiomaPreferido,
                TemaPreferido = usuario.TemaPreferido,
                BioAI = usuario.BioAI,
                Estado = usuario.Estado
            };
        }
    }
}
