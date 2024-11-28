using System.ComponentModel.DataAnnotations;

namespace Jobify.Api.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Surname { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Mail { get; set; } = null!;

        public bool? IsEmailVerified { get; set; }

        [Required]
        public UserTypeDTO UserType { get; set; } = null!;
    }
}
