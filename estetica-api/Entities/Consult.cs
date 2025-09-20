namespace dentist_panel_api.Entities;

public class Consult: AuditableEntity
{
    public ApplicationUser Owner { get; set; }
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; }
    public DateTime Date { get; set; }
    public decimal? Peso { get; set; }
    public decimal? Altura { get; set; }
    public decimal? Cintura { get; set; }
    public decimal? Cadera { get; set; }
    public decimal? Abdomen { get; set; }
    public string? Notes { get; set; }
    public Guid? OdontogramId { get; set; }
    public Odontogram? Odontogram { get; set; }
    public string? NotesFuture { get; set; }
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    public Guid? TipoDeTratamientoId { get; set; }
    public TipoDeTratamiento? TipoDeTratamiento { get; set; }
    public string Photo { get; set; }
}