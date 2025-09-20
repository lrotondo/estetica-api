using dentist_panel_api.Entities;

namespace dentist_panel_api.DTOs
{
    public class HealthCoverageDTO : AuditableEntity
    {
        public string Name { get; set; }
    }
}
