using System.ComponentModel.DataAnnotations;

namespace Jobify.Api.DTOs
{
    public class StudentRegistrationDTO
    {
        [Required]
        public UserDTO User { get; set; } = null!;

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "JMBAG must be exactly 10 characters long.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "JMBAG must contain only 10 numeric characters.")]
        public string? Jmbag { get; set; } = string.Empty;

        [Required]
        [Range(0.0, 5.0, ErrorMessage = "Average grade must be between 0.0 and 5.0.")]
        public decimal? AverageGrade { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Year of study must be a positive integer.")]
        public int? YearOfStudy { get; set; }
    }
}
