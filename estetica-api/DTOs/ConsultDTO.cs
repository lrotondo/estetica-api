using dentist_panel_api.Entities;
using dentist_panel_api.Entities.DTOs;

namespace dentist_panel_api.DTOs;

public class ConsultDTO: AuditableEntity
{
    public ApplicationUser Owner { get; set; }
    public Guid OwnerId { get; set; }
    public PatientDTO Patient { get; set; }
    public DateTime Date { get; set; }
    public decimal? Peso { get; set; }
    public decimal? Altura { get; set; }
    public decimal? Cintura { get; set; }
    public decimal? Cadera { get; set; }
    public decimal? Abdomen { get; set; }
    public string? Notes { get; set; }
    public OdontogramDTO? Odontogram { get; set; }
    public string? NotesFuture { get; set; }
    public DoctorDTO Doctor { get; set; }
    public TipoDeTratamientoDTO? TipoDeTratamiento { get; set; }
    public string Photo { get; set; }
}