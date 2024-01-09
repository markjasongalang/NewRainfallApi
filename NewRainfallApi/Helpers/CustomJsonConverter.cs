using NewRainfallApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NewRainfallApi.Helpers
{
    public class CustomJsonConverter : JsonConverter<RainfallReading>
    {
        public override RainfallReading Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;

                return new RainfallReading
                {
                    DateMeasured = root.GetProperty("dateTime").GetString()!,
                    AmountMeasured = root.GetProperty("value").GetDecimal()
                };
            }
        }

        public override void Write(Utf8JsonWriter writer, RainfallReading value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("dateMeasured", value.DateMeasured);
            writer.WriteNumber("amountMeasured", value.AmountMeasured);
            writer.WriteEndObject();
        }
    }
}
