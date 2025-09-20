using dentist_panel_api.Entities;

namespace dentist_panel_api.DTOs;

public class DecayDTO: AuditableEntity
{
    public int Quadrant { get; set; }
    public int ToothNumber { get; set; }
    public bool PreviouslyTreated { get; set; }
}