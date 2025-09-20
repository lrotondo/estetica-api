using dentist_panel_api.Entities;

namespace dentist_panel_api.DTOs;

public class OdontogramProsthesisDTO: AuditableEntity
{
    public int Start { get; set; }
    public int End { get; set; }
    public bool Removable { get; set; }
    public bool PreviouslyTreated { get; set; }
}