namespace dentist_panel_api.DTOs;

public class ConsultsFilter: BaseFilter
{
    public Guid? PatientId { get; set; }
    public Guid? DoctorId { get; set; }
    public Guid? HealthCoverageId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}