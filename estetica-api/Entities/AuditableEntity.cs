namespace dentist_panel_api.Entities
{
    public class AuditableEntity : IAuditableEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
