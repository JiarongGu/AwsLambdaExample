using Amazon.Lambda.Core;
using AwsLambdaExample.Application;
using AwsLambdaExample.Application.Exceptions;
using AwsLambdaExample.Lambda;
using AwsLambdaExample.Lambda.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sentry;
using System;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(JsonLambdaSerializer))]

namespace AwsLambdaExample.Lambda
{
    public class Function
    {
        public async Task<object?> FunctionHandler(FunctionInput input, ILambdaContext context)
        {
            var serviceProvier = Program.Instance.Value.ServiceProvider;
            return await LambdaFunction.HandleAsync(serviceProvier, input, context);
        }
    }

    public class LambdaFunction
    {
        public static async Task<object?> HandleAsync(IServiceProvider serviceProvider, FunctionInput input, ILambdaContext context)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                // setup context
                var actionContext = scope.ServiceProvider.GetRequiredService<IActionContext>();
                actionContext.LambdaContext = context;

                // process action
                var actionService = scope.ServiceProvider.GetRequiredService<IActionService>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Function>>();

                try
                {
                    return await actionService.Handle(input.Action, input.Model);
                }
                catch (KnownException ex)
                {
                    logger.LogInformation($"KnownException:: {ex.Message}", ex);
                    throw ex;
                }
                catch (Exception ex)
                {
                    logger.LogError($"Unkonwn Exception:: {ex.Message}", ex);
                    throw ex;
                }
                finally
                {
                    // adding flush delay time to avoid termination before sent to sentry in aws lambda.
                    // https://github.com/getsentry/sentry-dotnet/issues/126
                    await SentrySdk.FlushAsync(TimeSpan.FromSeconds(10));
                }
            }
        }
    }
}
