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
    public class HealthCoveragesController : ControllerBase
    {
        private readonly GenericServices _genericServices;
        private readonly HealthCoverageServices _healthCoverageServices;

        public HealthCoveragesController(GenericServices genericServices, HealthCoverageServices healthCoverageServices)
        {
            this._genericServices = genericServices;
            this._healthCoverageServices = healthCoverageServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<HealthCoverageDTO>>> Get()
        {
            return await _healthCoverageServices.Get();
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] HealthCoverageCreationDTO dto)
        {
            return await _genericServices.Create<HealthCoverage, HealthCoverageCreationDTO, HealthCoverageDTO>(dto);
        }
    }
}
