using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AwsLambdaExample.Application.Modules.TestExamples
{
    public class GetWeatherInput: IValidatable
    {
        public DateTime Date { get; set; }
    }

    [ScanAndRegister]
    public class GetWeather : IActionHandler
    {
        private readonly ISybaseContext _context;

        public GetWeather(ISybaseContext context)
        {
            _context = context;
        }

        public async Task<object?> HandleAsync(JObject? model)
        {
            var input = model.Parse<GetWeatherInput>().Validate();

            return new { Date = input.Date, Weather = "Sunny" };
        }
    }
}
