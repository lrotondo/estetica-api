using System.ComponentModel.DataAnnotations;

namespace dentist_panel_api.Entities
{
    public class Doctor : AuditableEntity
    {
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
