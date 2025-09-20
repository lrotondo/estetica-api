using dentist_panel_api.Entities;
using dentist_panel_api.Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace dentist_panel_api.Services
{
    public class TipoDeTratamientoServices
    {
        private readonly ApplicationDbContext _context;

        public TipoDeTratamientoServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TipoDeTratamientoDTO>> Get()
        {
            var tiposDeTratamiento = await _context.TiposDeTratamiento
                .OrderBy(t => t.Nombre)
                .ToListAsync();

            return tiposDeTratamiento.Select(t => new TipoDeTratamientoDTO
            {
                Id = t.Id,
                Nombre = t.Nombre,
                Descripcion = t.Descripcion,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList();
        }

        public async Task<TipoDeTratamientoDTO?> GetById(Guid id)
        {
            var tipoDeTratamiento = await _context.TiposDeTratamiento
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tipoDeTratamiento == null)
                return null;

            return new TipoDeTratamientoDTO
            {
                Id = tipoDeTratamiento.Id,
                Nombre = tipoDeTratamiento.Nombre,
                Descripcion = tipoDeTratamiento.Descripcion,
                CreatedAt = tipoDeTratamiento.CreatedAt,
                UpdatedAt = tipoDeTratamiento.UpdatedAt
            };
        }

        public async Task<TipoDeTratamientoDTO> Create(TipoDeTratamientoCreationDTO tipoDeTratamientoCreationDTO)
        {
            var tipoDeTratamiento = new TipoDeTratamiento
            {
                Nombre = tipoDeTratamientoCreationDTO.Nombre,
                Descripcion = tipoDeTratamientoCreationDTO.Descripcion
            };

            _context.TiposDeTratamiento.Add(tipoDeTratamiento);
            await _context.SaveChangesAsync();

            return new TipoDeTratamientoDTO
            {
                Id = tipoDeTratamiento.Id,
                Nombre = tipoDeTratamiento.Nombre,
                Descripcion = tipoDeTratamiento.Descripcion,
                CreatedAt = tipoDeTratamiento.CreatedAt,
                UpdatedAt = tipoDeTratamiento.UpdatedAt
            };
        }

        public async Task<TipoDeTratamientoDTO?> Update(Guid id, TipoDeTratamientoCreationDTO tipoDeTratamientoCreationDTO)
        {
            var tipoDeTratamiento = await _context.TiposDeTratamiento
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tipoDeTratamiento == null)
                return null;

            tipoDeTratamiento.Nombre = tipoDeTratamientoCreationDTO.Nombre;
            tipoDeTratamiento.Descripcion = tipoDeTratamientoCreationDTO.Descripcion;

            await _context.SaveChangesAsync();

            return new TipoDeTratamientoDTO
            {
                Id = tipoDeTratamiento.Id,
                Nombre = tipoDeTratamiento.Nombre,
                Descripcion = tipoDeTratamiento.Descripcion,
                CreatedAt = tipoDeTratamiento.CreatedAt,
                UpdatedAt = tipoDeTratamiento.UpdatedAt
            };
        }

        public async Task<bool> Delete(Guid id)
        {
            var tipoDeTratamiento = await _context.TiposDeTratamiento
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tipoDeTratamiento == null)
                return false;

            _context.TiposDeTratamiento.Remove(tipoDeTratamiento);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
