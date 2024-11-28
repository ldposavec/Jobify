using System.ComponentModel.DataAnnotations;

namespace Jobify.Api.DTOs
{
    public class UserTypeDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}