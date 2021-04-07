using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Weikio.ApiFramework.ApiProviders.PluginFramework;
using Weikio.ApiFramework.AspNetCore;
using Weikio.ApiFramework.AspNetCore.StarterKit;
using Weikio.ApiFramework.Core.Extensions;

namespace ApisAndEndpoints
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            Func<ILogger<Startup>, string, string> helloApi = (logger, name) =>
            {
                logger.LogInformation("Running the API using parameter {Name}", name);

                return $"Hello {name}!";
            };

            services.AddApiFrameworkStarterKit()
                .AddApi<CalculatorApi>()
                .AddEndpoint<CalculatorApi>("/mycal", 15)
                .AddEndpoint<CalculatorApi>("/second", 100)
                .AddEndpoint<CalculatorApi>("/third", 0)
                .AddApi(helloApi, "/myHello");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
