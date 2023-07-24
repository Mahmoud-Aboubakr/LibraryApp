using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Handlers
{
    public class CustomTimeConverter : JsonConverter<DateTime>
    {
        private const string TimeFormat = "HH:mm:ss";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString(), TimeFormat, null);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(TimeFormat));
        }
    }
}
