using System.ComponentModel.DataAnnotations;

namespace dentist_panel_api.Entities
{
    public class Appointment : AuditableEntity
    {
        public ApplicationUser Owner { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        public DateTime StartDate { get; set; }
        [Range(1, int.MaxValue)]
        public int Duration { get; set; }
        public string Notes { get; set; }       
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }

    }
}
