namespace dentist_panel_api.Entities;

public class OdontogramProsthesis: AuditableEntity
{
    public int Start { get; set; }
    public int End { get; set; }
    public bool Removable { get; set; }
    public bool PreviouslyTreated { get; set; }
}