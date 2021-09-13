using AwsLambdaExample.Application.Models;
using Newtonsoft.Json.Linq;

namespace AwsLambdaExample.Lambda.Models
{
    public class FunctionInput
    {
        public string Action { get; set; } = string.Empty;

        public JObject? Model { get; set; }
    }
}
