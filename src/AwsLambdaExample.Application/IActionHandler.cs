using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace AwsLambdaExample.Application
{
    public interface IActionHandler
    {
        Task<object?> HandleAsync(JObject? model);
    }
}
