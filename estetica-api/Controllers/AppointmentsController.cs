using dentist_panel_api.DTOs;
using dentist_panel_api.Entities;
using dentist_panel_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dentist_panel_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AppointmentsController : ControllerBase
    {
        private readonly GenericServices genericServices;
        private readonly AppointmentsServices appointmentsServices;
        private readonly UserServices userServices;

        public AppointmentsController(GenericServices genericServices, AppointmentsServices appointmentsServices, UserServices userServices)
        {
            this.genericServices = genericServices;
            this.appointmentsServices = appointmentsServices;
            this.userServices = userServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<AppointmentDTO>>> Get([FromQuery] AppointmentsFilter filter)
        {
            return await appointmentsServices.Get(filter, this.User);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AppointmentCreationDTO dto)
        {
            ApplicationUser user = await userServices.GetCurrentUser(this.User);
            dto.OwnerId = Guid.Parse(user.Id);
            dto.Owner = user;
            return await appointmentsServices.Create(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] Guid id, [FromBody] AppointmentPutDTO dto)
        {
            return await genericServices.Put<Appointment, AppointmentPutDTO>(id, dto);            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            return await genericServices.Delete<Appointment>(id);
        }

        [HttpPost("reminder")]
        [AllowAnonymous]
        public async Task<ActionResult> SendDailyReminders()
        {
            return await appointmentsServices.SendDailyReminders();
        }
    }
}
