namespace dentist_panel_api.Entities
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        Guid Id { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}