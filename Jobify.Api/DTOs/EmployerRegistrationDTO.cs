using System.ComponentModel.DataAnnotations;

namespace Jobify.Api.DTOs
{
    public class EmployerRegistrationDTO
    {
        [Required]
        public UserDTO User { get; set; } = null!;

        [Required]
        public FirmDTO Firm { get; set; } = null!;

        [Required]
        public string? Position { get; set; } = string.Empty;
    }
}
