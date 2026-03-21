using ApiSynchro.Data;
using ApiSynchro.DTOs;
using ApiSynchro.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiSynchro.Services
{
    public interface IIntencionBusquedaService
    {
        Task<List<IntencionBusquedaResponseDto>> ObtenerTodasAsync();
        Task<IntencionBusquedaResponseDto?> ObtenerPorIdAsync(int id);
        Task<IntencionBusquedaResponseDto> CrearAsync(IntencionBusquedaCreateDto dto);
        Task<IntencionBusquedaResponseDto?> ActualizarAsync(int id, IntencionBusquedaUpdateDto dto);
        Task<bool> EliminarAsync(int id);
    }

    public class IntencionBusquedaService : IIntencionBusquedaService
    {
        private readonly SynchroDbContext _context;

        public IntencionBusquedaService(SynchroDbContext context)
        {
            _context = context;
        }

        public async Task<List<IntencionBusquedaResponseDto>> ObtenerTodasAsync()
        {
            var intenciones = await _context.IntencionBusquedas.ToListAsync();
            return intenciones.Select(MapearADto).ToList();
        }

        public async Task<IntencionBusquedaResponseDto?> ObtenerPorIdAsync(int id)
        {
            var intencion = await _context.IntencionBusquedas.FindAsync(id);
            return intencion != null ? MapearADto(intencion) : null;
        }

        public async Task<IntencionBusquedaResponseDto> CrearAsync(IntencionBusquedaCreateDto dto)
        {
            var intencion = new IntencionBusqueda
            {
                Nombre = dto.Nombre,
                NombreEN = dto.NombreEN
            };

            _context.IntencionBusquedas.Add(intencion);
            await _context.SaveChangesAsync();

            return MapearADto(intencion);
        }

        public async Task<IntencionBusquedaResponseDto?> ActualizarAsync(int id, IntencionBusquedaUpdateDto dto)
        {
            var intencion = await _context.IntencionBusquedas.FindAsync(id);
            if (intencion == null)
                return null;

            if (!string.IsNullOrEmpty(dto.Nombre))
                intencion.Nombre = dto.Nombre;

            if (dto.NombreEN != null)
                intencion.NombreEN = dto.NombreEN;

            await _context.SaveChangesAsync();
            return MapearADto(intencion);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var intencion = await _context.IntencionBusquedas.FindAsync(id);
            if (intencion == null)
                return false;

            // Verificar si hay usuarios con esta intención
            var tieneUsuarios = await _context.Usuarios
                .AnyAsync(u => u.IntencionBusqueda == id);

            if (tieneUsuarios)
                return false; // No permitir eliminar si hay usuarios usando esta intención

            _context.IntencionBusquedas.Remove(intencion);
            await _context.SaveChangesAsync();
            return true;
        }

        private IntencionBusquedaResponseDto MapearADto(IntencionBusqueda intencion)
        {
            return new IntencionBusquedaResponseDto
            {
                IdIntencion = intencion.IdIntencion,
                Nombre = intencion.Nombre,
                NombreEN = intencion.NombreEN
            };
        }
    }
}
