using AwsLambdaExample.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AwsLambdaExample.Lambda
{
    public class Program
    {
        private Program() 
        {
            // current enviorment
            var currentEnvironment = Environment.GetEnvironmentVariable("DEPLOYMENT_STAGE") ?? "local";

            // configurations
            var configuration = new ConfigurationBuilder()
#if DEBUG
                .SetBasePath(Environment.CurrentDirectory)
#else
			    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
#endif
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{currentEnvironment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            var services = new ServiceCollection();

            var startup = new Startup(configuration);

            startup.ConfigureServices(services);

            CurrentEnvironment = currentEnvironment;
            Configuration = configuration;
            ServiceProvider = services.BuildServiceProvider();
        }

        public IConfiguration Configuration { get; }

        public string CurrentEnvironment { get; }

        public IServiceProvider ServiceProvider { get; }

        public static Lazy<Program> Instance = new Lazy<Program>(() => new Program());
    }
}
