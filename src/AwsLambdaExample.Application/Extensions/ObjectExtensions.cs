using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace AwsLambdaExample.Application
{
	public static class ObjectExtensions
	{
		public static T? Parse<T>(this object? value) where T : class, new()
		{
			if (value == null)
				return null;
			try
			{
				var json = JsonConvert.SerializeObject(value);
				var parsedObject = JsonConvert.DeserializeObject<T>(json);
				return parsedObject;
			}
			catch (JsonSerializationException)
			{
				// This will happen if any [JsonRequired] property is missing.
				return null;
			}
		}

		public static object? Parse(this object? value, Type type)
		{
			if (value == null)
				return null;

			try
			{
				var json = JsonConvert.SerializeObject(value);
				var parsedObject = JsonConvert.DeserializeObject(json, type);
				return parsedObject;
			}
			catch (JsonSerializationException)
			{
				// This will happen if any [JsonRequired] property is missing.
				return null;
			}
		}

		public static bool IsEmpty(this object value)
		{
			if (value == null)
			{
				return true;
			}
			try
			{
				return !(JObject.FromObject(value)?.Count > 0);
			}
			catch (Exception)
			{
				// it could be a non-JObject instance
				return true;
			}
		}
	}
}
