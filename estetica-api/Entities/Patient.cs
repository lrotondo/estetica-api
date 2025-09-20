namespace dentist_panel_api.Entities
{
    public class Patient : AuditableEntity
    {        
        public ApplicationUser Owner { get; set; }        
        public string Name { get; set; }
        public string AfipId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? Edad { get; set; }
        public int? CantidadHijos { get; set; }
        public string NotasGenerales { get; set; }

        public string NotasGeneralesAdicionales { get; set; }
        public DateTime LastWhatsAppInitialConversation { get; set; }

    }
}
