using System.ComponentModel.DataAnnotations;

namespace dentist_panel_api.Entities.DTOs
{
    public class DoctorDTO : AuditableEntity
    {
        // public ApplicationUser User { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
