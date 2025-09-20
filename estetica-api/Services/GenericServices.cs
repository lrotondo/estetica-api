using AutoMapper;
using dentist_panel_api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dentist_panel_api.Services
{
    public class GenericServices : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenericServices(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ActionResult> Create<Entity, CreationDTO, DTO>(CreationDTO creationDTO)
        {            
            var entity = mapper.Map<Entity>(creationDTO);
            await context.AddAsync(entity);
            await context.SaveChangesAsync();

            return Ok(mapper.Map<DTO>(entity));
        }

        public async Task<ActionResult> Delete<TEntity>(Guid id) where TEntity : class, IAuditableEntity, new()
        {
            var exists = await context.Set<TEntity>().AnyAsync(x => x.Id == id);
            if (!exists) return NotFound();

            context.Remove(new TEntity() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

        public async Task<ActionResult> Put<TEntity, TPutDTO>(Guid id, TPutDTO dto) where TEntity : class, IAuditableEntity, new()
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity == null) return NotFound();
            mapper.Map(dto, entity);            
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
