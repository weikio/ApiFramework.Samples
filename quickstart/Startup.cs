using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Weikio.ApiFramework.Abstractions;
using Weikio.ApiFramework.ApiProviders.PluginFramework;
using Weikio.ApiFramework.AspNetCore;
using Weikio.ApiFramework.AspNetCore.StarterKit;
using Weikio.ApiFramework.Core.Extensions;
using Weikio.ApiFramework.Plugins.OpenApi;
using Weikio.ApiFramework.Plugins.SqlServer;
using Weikio.ApiFramework.Plugins.SqlServer.Configuration;

namespace quickstart
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
            Func<ILogger<Startup>, string, string> helloApi = (logger, name) =>
            {
                logger.LogInformation("Running the API using parameter {Name}", name);
                return $"Hello {name}!";
            };
            
            services.AddControllers();

            services.AddApiFrameworkStarterKit()
                .AddApi(helloApi)
                .AddApi<CalculatorApi>()
                .AddSqlServer()
                .AddOpenApi()
                .AddEndpoint<CalculatorApi>("/mycal", 15)
                .AddEndpoint<CalculatorApi>("/second", 100)
                .AddEndpoint<CalculatorApi>("/third", 0)
                .AddEndpoint("/sqlserver", ("Weikio.ApiFramework.Plugins.SqlServer", Version.Parse("1.1.0.0")), new SqlServerOptions()
                {
                    ConnectionString =
                        "Server=tcp:adafydevtestdb001.database.windows.net,1433;User ID=docs;Password=3h1@*6PXrldU4F95;Integrated Security=false;Initial Catalog=adafyweikiodevtestdb001;"
                })
                .AddEndpoint("/sqlserver_products",  ("Weikio.ApiFramework.Plugins.SqlServer", Version.Parse("1.1.0.0")), new SqlServerOptions()
                {
                    ConnectionString =
                        "Server=tcp:adafydevtestdb001.database.windows.net,1433;User ID=docs;Password=3h1@*6PXrldU4F95;Integrated Security=false;Initial Catalog=adafyweikiodevtestdb001;",
                    Tables = new[] { "Product*" },
                })
                .AddEndpoint("/pets", "Weikio.ApiFramework.Plugins.OpenApi",
                    new ApiOptions() { SpecificationUrl = "https://petstore.swagger.io/v2/swagger.json", IncludeHttpMethods = new[] { "GET" } });
            
            var endpointDefinition = new EndpointDefinition("/pets_writeonly", "Weikio.ApiFramework.Plugins.OpenApi", 
                new ApiOptions()
                {
                    SpecificationUrl = "https://petstore.swagger.io/v2/swagger.json", ExcludeHttpMethods = new[] { "GET" }
                });

            services.AddSingleton(endpointDefinition);
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
    
    public class CalculatorApi
    {
        public int Configuration { get; set; }
        public int Sum(int x, int y)
        {
            return x + y + Configuration;
        }
    }
}
