using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LambdaStartup = AwsLambdaExample.Lambda.Startup;

namespace AwsLambdaExample.Local
{
    public class Startup
    {
        private readonly LambdaStartup _lambdaStartup;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _lambdaStartup = new LambdaStartup(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _lambdaStartup.ConfigureServices(services);
            services.AddTransient<LambdaFunctionMiddleware, LambdaFunctionMiddleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<LambdaFunctionMiddleware>();
        }
    }
}
