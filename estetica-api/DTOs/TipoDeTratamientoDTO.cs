namespace dentist_panel_api.Entities.DTOs
{
    public class TipoDeTratamientoDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
