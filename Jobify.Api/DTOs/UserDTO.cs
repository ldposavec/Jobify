using AutoMapper.Configuration.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Jobify.Api.DTOs
{
    public class UserDTO
    {
        [JsonIgnore]
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

        [Required]
        public string Password { get; set; } = null!;

        [JsonIgnore]
        public bool? IsEmailVerified { get; set; }

        [JsonIgnore]
        public UserTypeDTO UserType { get; set; } = null!;
    }
}
