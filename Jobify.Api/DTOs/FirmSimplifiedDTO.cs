using System.ComponentModel.DataAnnotations;

namespace Jobify.Api.DTOs
{
    public class FirmSimplifiedDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string FirmName { get; set; } = null!;

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "OIB must be exactly 11 characters long.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "OIB must contain only numeric characters.")]
        public string Oib { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = null!;

        [StringLength(100)]
        public string? Industry { get; set; }

        public double? AverageGrade { get; set; }
    }
}
