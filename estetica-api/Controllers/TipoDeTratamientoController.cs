using dentist_panel_api.Entities.DTOs;
using dentist_panel_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dentist_panel_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TipoDeTratamientoController : ControllerBase
    {
        private readonly TipoDeTratamientoServices _tipoDeTratamientoServices;

        public TipoDeTratamientoController(TipoDeTratamientoServices tipoDeTratamientoServices)
        {
            _tipoDeTratamientoServices = tipoDeTratamientoServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipoDeTratamientoDTO>>> Get()
        {
            var tiposDeTratamiento = await _tipoDeTratamientoServices.Get();
            return Ok(tiposDeTratamiento);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoDeTratamientoDTO>> GetById(Guid id)
        {
            var tipoDeTratamiento = await _tipoDeTratamientoServices.GetById(id);
            
            if (tipoDeTratamiento == null)
                return NotFound();

            return Ok(tipoDeTratamiento);
        }

        [HttpPost]
        public async Task<ActionResult<TipoDeTratamientoDTO>> Create(TipoDeTratamientoCreationDTO tipoDeTratamientoCreationDTO)
        {
            var tipoDeTratamiento = await _tipoDeTratamientoServices.Create(tipoDeTratamientoCreationDTO);
            return CreatedAtAction(nameof(GetById), new { id = tipoDeTratamiento.Id }, tipoDeTratamiento);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TipoDeTratamientoDTO>> Update(Guid id, TipoDeTratamientoCreationDTO tipoDeTratamientoCreationDTO)
        {
            var tipoDeTratamiento = await _tipoDeTratamientoServices.Update(id, tipoDeTratamientoCreationDTO);
            
            if (tipoDeTratamiento == null)
                return NotFound();

            return Ok(tipoDeTratamiento);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _tipoDeTratamientoServices.Delete(id);
            
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
