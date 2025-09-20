using System.ComponentModel.DataAnnotations;

namespace dentist_panel_api.DTOs
{
    public class AppointmentsFilter
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
