using NewRainfallApi.Helpers;
using System.Text.Json.Serialization;

namespace NewRainfallApi.Models
{
    [JsonConverter(typeof(CustomJsonConverter))]
    public class RainfallReading
    {
        public string DateMeasured { get; set; } = null!;
        public decimal AmountMeasured { get; set; }
    }
}
