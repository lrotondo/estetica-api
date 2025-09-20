using System.ComponentModel.DataAnnotations;

namespace dentist_panel_api.DTOs
{
    public class SignInDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
