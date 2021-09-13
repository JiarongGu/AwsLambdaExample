using AwsLambdaExample.Application;
using AwsLambdaExample.Application.Exceptions;
using AwsLambdaExample.Application.Models;
using AwsLambdaExample.Application.Models.Options;
using EntityFrameworkCore.Ase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Reflection;

namespace AwsLambdaExample.Lambda
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var deploymentStage = DeploymentStage.FromValue(GetEnvironmentVariableOrDefault("DEPLOYMENT_STAGE", "local"));
            var sybaseConnection = GetEnvironmentVariableOrDefault("CONNECTION_STRING");

            Console.WriteLine($"DeploymentStage:: {deploymentStage}");

            var environmentOptions = new EnvironmentOptions
            {
                DeploymentStage = deploymentStage,
                RegionName = Environment.GetEnvironmentVariable("AWS_REGION_NAME") ?? "ap-southeast-2"
            };


            // configure entityframework
            services.AddDbContext<ISybaseContext, SybaseContext>(x => x.UseAse(sybaseConnection));

            // configure sybaseJconnect
            services.ScanAndRegister(Assembly.GetAssembly(typeof(ServiceCollectionExtensions))!);
            services.ScanAndRegister(Assembly.GetAssembly(typeof(Program))!);

            services.Configure<EnvironmentOptions>(options =>
            {
                options.DeploymentStage = environmentOptions.DeploymentStage;
                options.RegionName = environmentOptions.RegionName;
            });

            // configure logger
            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.Console(
                    theme: AnsiConsoleTheme.Code,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}",
                    restrictedToMinimumLevel: LogEventLevel.Information
                 )
                .WriteTo.Sentry(
                    dsn: "https://3290c8cec77844cf99f11e37dbfe40a5@o372200.ingest.sentry.io/5655962",
                    minimumBreadcrumbLevel: LogEventLevel.Warning,
                    minimumEventLevel: LogEventLevel.Warning,
                    environment: environmentOptions.DeploymentStage.Value.ToUpper(),
                    attachStackTrace: true
                );

            Log.Logger = loggerConfiguration.CreateLogger();

            services.AddLogging(builder => builder.AddSerilog());

            return services;
        }

        private string GetEnvironmentVariableOrDefault(string key, string? defaultValue = null) 
        {
            var value = Environment.GetEnvironmentVariable(key);

            if (value != null) 
            {
                return value;
            }

            if (defaultValue != null)
            {
                return defaultValue;
            }

            throw new KnownException($"missing env: {key}");
        }
}
}
