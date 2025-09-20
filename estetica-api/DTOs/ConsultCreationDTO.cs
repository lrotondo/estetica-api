using dentist_panel_api.Entities;

namespace dentist_panel_api.DTOs;

public class ConsultCreationDTO
{
    public ApplicationUser Owner { get; set; }
    public Guid OwnerId { get; set; }
    public Guid PatientId { get; set; }
    public DateTime Date { get; set; }
    public decimal? Peso { get; set; }
    public decimal? Altura { get; set; }
    public decimal? Cintura { get; set; }
    public decimal? Cadera { get; set; }
    public decimal? Abdomen { get; set; }
    public string? Notes { get; set; }
    public OdontogramCreationDTO? Odontogram { get; set; }
    public string? NotesFuture { get; set; }
    public string Photo { get; set; }
    public Guid DoctorId { get; set; }
    public Guid? TipoDeTratamientoId { get; set; }
}