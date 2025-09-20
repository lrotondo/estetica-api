using System.ComponentModel.DataAnnotations.Schema;

namespace dentist_panel_api.Entities;

public class Odontogram: AuditableEntity
{
    public List<Decay> Decays { get; set; }
    public string CrownsStore { get; set; }
    [NotMapped]
    public virtual List<int> Crowns
    {
        get => (CrownsStore != null && CrownsStore.Length > 0) ? CrownsStore.Split(",").Select(int.Parse).ToList() : new List<int>();
        set => CrownsStore = String.Join(",", value);
    }
    
    public string CanceledStore { get; set; }
    [NotMapped]
    public virtual List<int> Canceled
    {
        get => (CanceledStore != null && CanceledStore.Length > 0) ? CanceledStore.Split(",").Select(int.Parse).ToList() : new List<int>();
        set => CanceledStore = String.Join(",", value);
    }
    
    public List<OdontogramProsthesis> Prostheses { get; set; }
}