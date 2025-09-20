using AutoMapper;
using dentist_panel_api.DTOs;
using dentist_panel_api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Security.Claims;

namespace dentist_panel_api.Services
{
    public class PatientsServices : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserServices _userServices;
        private readonly IMapper _mapper;

        public PatientsServices(ApplicationDbContext context, IMapper mapper, UserServices userServices)
        {
            this._context = context;
            this._mapper = mapper;
            this._userServices = userServices;
        }

        public async Task<ActionResult<ListResult<PatientDTO>>> Get(PatientsFilter filterDTO, ClaimsPrincipal claims)
        {
            ApplicationUser user = await _userServices.GetCurrentUser(claims);
            var patients = _context.Patients.Where(p => p.Owner.Id.Equals(user.Id)).OrderBy(p => p.Name).AsQueryable();
            //var patients = _context.Patients.OrderBy(p => p.Name).Include(p => p.HealthCoverage).AsQueryable();
            if (!string.IsNullOrEmpty(filterDTO.Filter))
            {
                patients = patients.Where(p => p.Name.ToLower().Contains(filterDTO.Filter.ToLower()) || p.AfipId.Contains(filterDTO.Filter)).AsQueryable();
            }           

            if (filterDTO.ConsultFrom != null)
            {
                var date = DateTime.SpecifyKind(filterDTO.ConsultFrom ?? DateTime.Now, DateTimeKind.Utc);
                var consults = _context.Consults.Where(c => c.Date >= date).AsQueryable();
                patients = patients.Where(p => consults.Any(c => c.PatientId == p.Id)).AsQueryable();
            }            
            if (filterDTO.ConsultTo != null)
            {
                var date = DateTime.SpecifyKind(filterDTO.ConsultTo ?? DateTime.Now, DateTimeKind.Utc);
                var consults = _context.Consults.Where(c => c.Date <= date).AsQueryable();
                patients = patients.Where(p => consults.Any(c => c.PatientId == p.Id)).AsQueryable();
            }
            int count = patients.AsQueryable().Count();
            patients = patients.Skip(filterDTO.Page * filterDTO.PerPage).Take(filterDTO.PerPage).AsQueryable();
            if (filterDTO.Sort != null)
            {
                var order = filterDTO.Sort.IsAscending ? "" : "descending";
                patients = patients.OrderBy($"{filterDTO.Sort.Field} {order}");
            }
            return new ListResult<PatientDTO>(count, _mapper.Map<List<PatientDTO>>(patients));
        }

        public async Task<ActionResult> Put(Guid id, PatientCreationDTO dto)
        {
            var entity = await _context.Patients.FindAsync(id);
            if (entity == null) return NotFound();
            entity.Name = dto.Name;
            entity.Email = dto.Email;
            entity.AfipId = dto.AfipId;            
            entity.LastWhatsAppInitialConversation = dto.LastWhatsAppInitialConversation;
            entity.PhoneNumber = dto.PhoneNumber;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
