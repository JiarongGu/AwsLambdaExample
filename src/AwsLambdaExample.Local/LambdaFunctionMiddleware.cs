using AwsLambdaExample.Lambda;
using AwsLambdaExample.Lambda.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AwsLambdaExample.Local
{
    public class LambdaFunctionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _provider;

        public LambdaFunctionMiddleware(RequestDelegate next, IServiceProvider provider)
        {
            _next = next;
            _provider = provider;
        }

        public async Task Invoke(HttpContext context)
        {
            using (var scope = _provider.CreateScope())
            {
                FunctionInput input;

                using (var stream = new StreamReader(context.Request.Body))
                {
                    var body = await stream.ReadToEndAsync();
                    input = JsonConvert.DeserializeObject<FunctionInput>(body);
                }

                var response = await LambdaFunction.HandleAsync(_provider, input, null);
                var json = JsonConvert.SerializeObject(response, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });
                await context.Response.WriteAsync(json);
            }
        }
    }
}
