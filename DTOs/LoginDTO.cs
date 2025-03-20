using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.DTOs
{
    public class LoginDTO
    {
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 12 characters.")]
        public string Password { get; set; }
    }
}
