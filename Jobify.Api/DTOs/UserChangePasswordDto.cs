using System.ComponentModel.DataAnnotations;

namespace Jobify.Api.DTOs
{
    public class UserChangePasswordDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm password is required.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
