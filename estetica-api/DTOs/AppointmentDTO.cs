using dentist_panel_api.Entities;
using dentist_panel_api.Entities.DTOs;
using System.ComponentModel.DataAnnotations;

namespace dentist_panel_api.DTOs
{
    public class AppointmentDTO : AuditableEntity
    {
        public ApplicationUser Owner { get; set; }
        public Guid OwnerId { get; set; }
        public PatientDTO Patient { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public string Notes { get; set; }
        public Guid DoctorId { get; set; }
        public DoctorDTO Doctor { get; set; }
    }
}
