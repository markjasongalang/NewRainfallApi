using System.Text.Json.Serialization;

namespace NewRainfallApi.Models
{
    public class RainfallReadingResponse
    {
        [JsonPropertyName("items")]
        public List<RainfallReading> Readings { get; set; } = null!;
    }
}
