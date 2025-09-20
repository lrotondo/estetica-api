using Microsoft.AspNetCore.Identity;

namespace dentist_panel_api.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public int PlanType { get; set; }
        public string CodePhoneId { get; set; }
    }
}
