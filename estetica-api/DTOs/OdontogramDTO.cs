using dentist_panel_api.Entities;

namespace dentist_panel_api.DTOs;

public class OdontogramDTO: AuditableEntity
{
    public List<DecayDTO> Decays { get; set; }
    public List<int> Canceled { get; set; }
    public List<int> Crowns { get; set; }
    public List<OdontogramProsthesisDTO> Prostheses { get; set; }
}