using BrunoZell.ModelBinding;
using dentist_panel_api.DTOs;
using dentist_panel_api.Entities;
using dentist_panel_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Linq;

namespace dentist_panel_api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ConsultsController : ControllerBase
{
    private readonly GenericServices _genericServices;
    private readonly ConsultsServices _consultsServices;
    private readonly UserServices _userServices;
    private readonly AWSBucketServices _awsServices;

    public ConsultsController(GenericServices genericServices, ConsultsServices consultsServices, UserServices userServices, AWSBucketServices awsServices)
    {
        _genericServices = genericServices;
        _consultsServices = consultsServices;
        this._userServices = userServices;
        this._awsServices = awsServices;
    }
    
    [HttpGet]
    public async Task<ActionResult<ListResult<ConsultDTO>>> GetConsults([FromQuery] ConsultsFilter filter)
    {
        return await _consultsServices.Get(filter, this.User);
    }

    /*[HttpPost]
    public async Task<ActionResult<ConsultDTO>> CreateConsultTest([ModelBinder(BinderType = typeof(JsonModelBinder))] ConsultCreationDTO value,
    [FromForm] IList<IFormFile> files)
    {
        
        ApplicationUser user = await _userServices.GetCurrentUser(this.User);
        value.OwnerId = Guid.Parse(user.Id);
        value.Owner = user;
        return await _genericServices.Create<Consult, ConsultCreationDTO, ConsultDTO>(value);
    }*/

    [HttpPost]
    public async Task<ActionResult<ConsultDTO>> CreateConsult(ConsultCreationDTO dto)
    {
        ApplicationUser user = await _userServices.GetCurrentUser(this.User);
        dto.OwnerId = Guid.Parse(user.Id);
        dto.Owner = user;
        return await _consultsServices.Create(dto);
    }

    [HttpGet("last-odontogram/{patientId}")]
    public async Task<ActionResult<DateTime>> GetPatientLastOdontogram([FromRoute] Guid patientId)
    {
        return await _consultsServices.GetPatientLastOdontogram(patientId);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateConsult([FromRoute] Guid id, [FromBody] ConsultCreationDTO dto)
    {
        //return await _genericServices.Put<Consult, ConsultCreationDTO>(id, dto);
        return await _consultsServices.Put(id, dto);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteConsult([FromRoute] Guid id)
    {
        return await _genericServices.Delete<Consult>(id);
    }
}