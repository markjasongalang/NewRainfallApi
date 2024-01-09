using NewRainfallApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NewRainfallApi.Helpers
{
    // Custom JSON converter for RainfallReading objects
    public class CustomJsonConverter : JsonConverter<RainfallReading>
    {
        // Deserialize JSON to RainfallReading
        public override RainfallReading Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;

                // Extract properties from JSON and create RainfallReading object
                return new RainfallReading
                {
                    DateMeasured = root.GetProperty("dateTime").GetString()!,
                    AmountMeasured = root.GetProperty("value").GetDecimal()
                };
            }
        }

        // Serialize RainfallReading to JSON
        public override void Write(Utf8JsonWriter writer, RainfallReading value, JsonSerializerOptions options)
        {
            // Write RainfallReading properties to JSON
            writer.WriteStartObject();
            writer.WriteString("dateMeasured", value.DateMeasured);
            writer.WriteNumber("amountMeasured", value.AmountMeasured);
            writer.WriteEndObject();
        }
    }
}
