using System.Linq.Dynamic.Core;
using System.Security.Claims;
using AutoMapper;
using dentist_panel_api.DTOs;
using dentist_panel_api.Entities;
using dentist_panel_api.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dentist_panel_api.Services;

public class DoctorsServices: ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserServices _userServices;
    private readonly IMapper _mapper;

    public DoctorsServices(ApplicationDbContext context, IMapper mapper, UserServices userServices)
    {
        _context = context;
        _mapper = mapper;
        this._userServices = userServices;
    }
    
    public async Task<ActionResult<List<DoctorDTO>>> Get(ClaimsPrincipal claims)
    {
        ApplicationUser user = await _userServices.GetCurrentUser(claims);
        var doctors = _context.Doctors
            .OrderByDescending(p => p.CreatedAt)            
            .Where(c => c.UserId == user.Id)
            .AsQueryable();
        
        doctors = doctors.AsQueryable();        
        return new List<DoctorDTO>(_mapper.Map<List<DoctorDTO>>(doctors));
    }

    
}