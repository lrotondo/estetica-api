namespace dentist_panel_api.DTOs
{
    public class AppointmentPutDTO
    {
        public Guid PatientId { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public string Notes { get; set; }
        public Guid DoctorId { get; set; }
       
    }
}
