using System.Configuration;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using AutoMapper;
using dentist_panel_api.DTOs;
using dentist_panel_api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;

namespace dentist_panel_api.Services;

public class ConsultsServices: ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly UserServices _userServices;
    private readonly IMapper _mapper;

    public ConsultsServices(ApplicationDbContext context, IMapper mapper, UserServices userServices, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        this._userServices = userServices;
        this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    
    public async Task<ActionResult<ListResult<ConsultDTO>>> Get(ConsultsFilter filterDTO, ClaimsPrincipal claims)
    {
        ApplicationUser user = await _userServices.GetCurrentUser(claims);
        var consults = _context.Consults
            .OrderByDescending(p => p.CreatedAt)
            .Include(c => c.Doctor)
            .Include(c => c.TipoDeTratamiento)
            .Include(c => c.Odontogram)
            .ThenInclude(o => o.Prostheses)
            .Include(c => c.Odontogram)
            .ThenInclude(o => o.Decays)
            .Include(c => c.Patient)            
            .Where(c => c.Owner.Id == user.Id)
            .AsQueryable();
        
        if (filterDTO.PatientId != null)
        {
            consults = consults.Where(c => c.PatientId == filterDTO.PatientId);
        }
        
        if (filterDTO.DoctorId != null)
        {
            consults = consults.Where(c => c.DoctorId == filterDTO.DoctorId);
        }

        if (filterDTO.From != null)
        {                
            var date = DateTime.SpecifyKind(filterDTO.From ?? DateTime.Now, DateTimeKind.Utc);
            consults = consults.Where(c => c.Date >= date);
        }
        
        if (filterDTO.To != null)
        {
            var date = DateTime.SpecifyKind(filterDTO.To ?? DateTime.Now, DateTimeKind.Utc);
            consults = consults.Where(c => c.Date <= date);
        }
        int count = consults.Count();
        consults = consults.Skip(filterDTO.Page * filterDTO.PerPage).Take(filterDTO.PerPage).AsQueryable();
        if (filterDTO.Sort != null)
        {
            var order = filterDTO.Sort.IsAscending ? "" : "descending";
            consults = consults.OrderBy($"{filterDTO.Sort.Field} {order}");
        }

        return new ListResult<ConsultDTO>(count, _mapper.Map<List<ConsultDTO>>(consults));
    }

    public async Task<ActionResult<DateTime>> GetPatientLastOdontogram(Guid patientId)
    {
        var consults = await _context.Consults
            .Where(c => c.PatientId == patientId && c.OdontogramId != null)
            .OrderBy(c => c.Date)
            .ToListAsync();

        if (consults.Count() < 1) return NoContent();
        
        return Ok(consults.Last().Date);
    }

    public async Task<ActionResult> Put(Guid id, ConsultCreationDTO dto)
    {
        var entity = await _context.Consults.Include(c => c.Odontogram).FirstOrDefaultAsync(c => c.Id == id);
        if (entity == null) return NotFound();
        entity.PatientId = dto.PatientId;
        entity.DoctorId = dto.DoctorId;
        entity.TipoDeTratamientoId = dto.TipoDeTratamientoId;
        entity.Date = dto.Date;
        entity.Peso = dto.Peso;
        entity.Altura = dto.Altura;
        entity.Cintura = dto.Cintura;
        entity.Cadera = dto.Cadera;
        entity.Abdomen = dto.Abdomen;
        entity.Notes = dto.Notes;
        entity.NotesFuture = dto.NotesFuture;
        var sameFile = false;
        if (dto.Photo!=null)
            sameFile = dto.Photo.IndexOf(",") < 0;
        if (entity.Photo!=null && (dto.Photo==null || dto.Photo == "") || !sameFile)
        {
            if (entity.Photo != "")
                Helper.DeleteObjectFromBucket(dto.Owner.Id + "/" + entity.Id + ".jpg", this._configuration);
            entity.Photo = "";
        }

        if (dto.Odontogram!=null)
        {
            _mapper.Map(dto.Odontogram, entity.Odontogram);
            _context.Odontograms.Update(entity.Odontogram);

        }
        if (!sameFile && (dto.Photo != null && dto.Photo != ""))
        {
            var bytes = Convert.FromBase64String(dto.Photo.Split(',')[1]);
            var contentType = dto.Photo.Split(';')[0].Replace("data:", "");

            using (var ms = new MemoryStream(bytes))
            {
                FormFile file = new FormFile(ms, 0, bytes.Length, "file", "file")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType
                };
                await Helper.UploadObjectToBucket(dto.Owner.Id + "/" + entity.Id + ".jpg", this._configuration, file);
                entity.Photo = dto.Owner.Id + "/" + entity.Id + ".jpg";
            }
        }
        _context.Consults.Update(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    public async Task<ActionResult> Create(ConsultCreationDTO creationDTO)
    {
        var entity = _mapper.Map<Consult>(creationDTO);        
        await _context.AddAsync(entity);
        entity.Photo = creationDTO.Owner.Id + "/" + entity.Id + ".jpg";
        await _context.SaveChangesAsync();

        if (creationDTO.Photo!=null && creationDTO.Photo != "")
        {
            var bytes = Convert.FromBase64String(creationDTO.Photo.Split(',')[1]);
            var contentType = creationDTO.Photo.Split(';')[0].Replace("data:", "");

            using (var ms = new MemoryStream(bytes))
            {                            
                FormFile file = new FormFile(ms, 0, bytes.Length, "file", "file")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType
                };
                await Helper.UploadObjectToBucket(creationDTO.Owner.Id + "/" + entity.Id + ".jpg", this._configuration, file);
            }
        }       
        return Ok(_mapper.Map<ConsultDTO>(entity));
    }
}