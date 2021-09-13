using Newtonsoft.Json;
using System;

namespace AwsLambdaExample.Application
{
    public class TrimmingConverter : JsonConverter
    {
        public override bool CanRead => true;

        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return ((string?)reader.Value)?.Trim();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var trimmedValue = ((string?)value)?.Trim();
            writer.WriteValue(trimmedValue);
        }
    }
}
