using dentist_panel_api.DTOs;
using dentist_panel_api.Entities;
using dentist_panel_api.Entities.DTOs;
using dentist_panel_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dentist_panel_api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DoctorsController : ControllerBase
{
    private readonly GenericServices _genericServices;
    private readonly DoctorsServices _doctorsServices;
    private readonly UserServices _userServices;

    public DoctorsController(GenericServices genericServices, DoctorsServices doctorsServices, UserServices userServices)
    {
        _genericServices = genericServices;
        _doctorsServices = doctorsServices;
        this._userServices = userServices;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<DoctorDTO>>> GetDoctors()
    {
        return await _doctorsServices.Get(this.User);
    }
    
}