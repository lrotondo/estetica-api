namespace dentist_panel_api.Entities;

public class Decay: AuditableEntity
{
    public int Quadrant { get; set; }
    public int ToothNumber { get; set; }
    public bool PreviouslyTreated { get; set; }
}