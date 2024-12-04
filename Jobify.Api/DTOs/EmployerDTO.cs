using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Jobify.Api.DTOs
{
    public class EmployerDTO
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public UserDTO User { get; set; } = null!;

        [Required]
        public FirmDTO Firm { get; set; } = null!; 

        [StringLength(100)]
        public string? Position { get; set; }

        public List<JobAdDTO> JobAds { get; set; } = new();
    }
}
