using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Jobify.Api.DTOs
{
    public class JobAppDTO
    {
        [JsonIgnore]  
        public int Id { get; set; }

        [Required]
        public JobAdDTO JobAd { get; set; } = null!;

        [Required]
        public StudentDTO Student { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "CV filepath must be at most 500 characters.")]
        public string CvFilepath { get; set; } = null!;

        [Required]
        public StatusDTO Status { get; set; } = null!;
    }
}
