using System.ComponentModel.DataAnnotations;

namespace Jobify.Api.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int FirmId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Grade must be between 1 and 5.")]
        public int Grade { get; set; }

        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string? Comment { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
