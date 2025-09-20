namespace dentist_panel_api.DTOs;

public class OdontogramCreationDTO
{
    public List<DecayCreationDTO> Decays { get; set; }
    public List<int> Canceled { get; set; }
    public List<int> Crowns { get; set; }
    public List<OdontogramProsthesisCreationDTO> Prostheses { get; set; }
}