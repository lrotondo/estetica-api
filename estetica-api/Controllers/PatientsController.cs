using dentist_panel_api.DTOs;
using dentist_panel_api.Entities;
using dentist_panel_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace dentist_panel_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PatientsController : ControllerBase
    {
        private readonly GenericServices genericServices;
        private readonly PatientsServices patientsServices;
        private readonly UserServices userServices;

        public PatientsController(GenericServices genericServices, PatientsServices patientsServices, UserServices userServices)
        {
            this.genericServices = genericServices;
            this.patientsServices = patientsServices;
            this.userServices = userServices;
        }

        [HttpGet]
        public async Task<ActionResult<ListResult<PatientDTO>>> Get([FromQuery] PatientsFilter filterDTO)
        {         
            return await patientsServices.Get(filterDTO, this.User);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] PatientCreationDTO dto)
        {
            ApplicationUser user = await userServices.GetCurrentUser(this.User);
            dto.OwnerId = Guid.Parse(user.Id);
            dto.Owner = user;
            return await genericServices.Create<Patient, PatientCreationDTO, PatientDTO>(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] Guid id, [FromBody] PatientCreationDTO dto)
        {
            return await patientsServices.Put(id, dto);
            //return await genericServices.Put<Patient, PatientCreationDTO>(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            return await genericServices.Delete<Patient>(id);
        }
    }
}
