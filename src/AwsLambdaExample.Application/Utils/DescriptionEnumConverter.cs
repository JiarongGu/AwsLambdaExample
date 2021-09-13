using AwsLambdaExample.Application.Utils;
using Newtonsoft.Json;
using System;

namespace AwsLambdaExample.Application
{
    public class DescriptionEnumConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var description = EnumExtensions.GetEnumDescription(value, value.GetType());

            writer.WriteValue(description);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                if (!ReflectionUtils.IsNullableType(objectType))
                {
                    throw new JsonSerializationException($"Cannot convert null value to {objectType.Name}.");
                }

                return null;
            }

            var isNullable = ReflectionUtils.IsNullableType(objectType);
            var type = isNullable ? Nullable.GetUnderlyingType(objectType)! : objectType;
            string? enumText = reader.Value?.ToString();

            try
            {
                if (string.IsNullOrEmpty(enumText)) 
                {
                    return null;
                }
                return EnumExtensions.ParseEnumDescription(enumText, type);
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException($"Error converting value {enumText} to type '{type.Name}'.", ex);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            var type = ReflectionUtils.IsNullableType(objectType)
                ? Nullable.GetUnderlyingType(objectType)!
                : objectType;

            return type.IsEnum;
        }
    }
}
