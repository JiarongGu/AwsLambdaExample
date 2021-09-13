using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;

namespace AwsLambdaExample.Application
{
    public interface IActionContext 
    {
        ILambdaContext? LambdaContext { get; set; }
    }

    [ScanAndRegister(Lifetime = ServiceLifetime.Scoped)]
    public class ActionContext: IActionContext
    {
        public ILambdaContext? LambdaContext { get; set; }
    }
}
