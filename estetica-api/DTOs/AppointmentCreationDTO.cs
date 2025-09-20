using dentist_panel_api.Entities;

namespace dentist_panel_api.DTOs
{
    public class AppointmentCreationDTO
    {
        public ApplicationUser Owner { get; set; }
        public Guid OwnerId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public string Notes { get; set; }
        
    }
}
