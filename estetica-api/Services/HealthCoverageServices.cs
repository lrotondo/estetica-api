using AutoMapper;
using dentist_panel_api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dentist_panel_api.Services
{
    public class HealthCoverageServices : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public HealthCoverageServices(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ActionResult<List<HealthCoverageDTO>>> Get()
        {
            var healthCoverages = await context.HealthCoverages.ToListAsync();
            return mapper.Map<List<HealthCoverageDTO>>(healthCoverages);
        }
    }
}
