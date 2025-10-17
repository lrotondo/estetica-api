using dentist_panel_api.Entities;

namespace dentist_panel_api.DTOs
{
    public class PatientDTO : AuditableEntity
    {
        public ApplicationUserDTO Owner { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public string AfipId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? Edad { get; set; }
        public int? CantidadHijos { get; set; }
        public string NotasGenerales { get; set; }
        public DateTime LastWhatsAppInitialConversation { get; set; }
    }
}
