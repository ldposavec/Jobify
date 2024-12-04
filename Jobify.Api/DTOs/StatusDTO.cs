using System.Text.Json.Serialization;

namespace Jobify.Api.DTOs
{
    public class StatusDTO
    {
        [JsonIgnore]  
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
