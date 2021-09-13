using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

namespace AwsLambdaExample.Lambda
{
    /// <summary>
    /// https://github.com/aws/aws-lambda-dotnet/blob/master/Libraries/src/Amazon.Lambda.Serialization.Json/JsonSerializer.cs
    /// </summary>
    public class JsonLambdaSerializer : ILambdaSerializer
    {
        private JsonSerializer serializer;

        public JsonLambdaSerializer()
        {
            var settings = new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            serializer = JsonSerializer.Create(settings);
        }

        public void Serialize<T>(T response, Stream responseStream)
        {
            try
            {
                StreamWriter writer = new StreamWriter(responseStream);
                serializer.Serialize(writer, response);
                writer.Flush();
            }
            catch (Exception e)
            {
                throw new JsonSerializerException($"Error converting the response object of type {typeof(T).FullName} from the Lambda function to JSON: {e.Message}", e);
            }
        }

        public T Deserialize<T>(Stream requestStream)
        {
            try
            {
                TextReader reader = new StreamReader(requestStream);
                JsonReader jsonReader = new JsonTextReader(reader);
                return serializer.Deserialize<T>(jsonReader)!;
            }
            catch (Exception e)
            {
                string message;
                var targetType = typeof(T);
                if (targetType == typeof(string))
                {
                    message = $"Error converting the Lambda event JSON payload to a string. JSON strings must be quoted, for example \"Hello World\" in order to be converted to a string: {e.Message}";
                }
                else
                {
                    message = $"Error converting the Lambda event JSON payload to type {targetType.FullName}: {e.Message}";
                }
                throw new JsonSerializerException(message, e);
            }
        }
    }
}
