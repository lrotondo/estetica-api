namespace dentist_panel_api.DTOs
{
    public class PatientsFilter : BaseFilter
    {
        public string Filter { get; set; }
        public Guid? HealthCoverageId { get; set; }
        public DateTime? ConsultFrom { get; set; }
        public DateTime? ConsultTo { get; set; }
    }
}
