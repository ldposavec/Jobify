using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Jobify.Api.DTOs
{
    public class AdminDTO
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public UserDTO User { get; set; } = null!;
    }
}
