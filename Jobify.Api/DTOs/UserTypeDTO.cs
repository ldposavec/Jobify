using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Jobify.Api.DTOs
{
    public class UserTypeDTO
    {
        //[JsonIgnore]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}