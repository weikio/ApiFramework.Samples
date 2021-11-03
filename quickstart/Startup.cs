using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Weikio.ApiFramework;
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
            services.AddControllers();

            Func<ILogger<Startup>, string, string> helloApi = (logger, name) =>
            {
                logger.LogInformation("Running the API using parameter {Name}", name);
                return $"Hello {name}!";
            };

            services.AddApiFrameworkStarterKit()
                .AddApi(helloApi)
                .AddApi<CalculatorApi>()
                .AddSqlServer("/sqlserver",
                    new SqlServerOptions
                    {
                        ConnectionString =
                            "Server=tcp:adafydevtestdb001.database.windows.net,1433;User ID=docs;Password=3h1@*6PXrldU4F95;Integrated Security=false;Initial Catalog=adafyweikiodevtestdb001;"
                    })
                .AddSqlServer("/sqlserver_products",
                    new SqlServerOptions
                    {
                        ConnectionString =
                            "Server=tcp:adafydevtestdb001.database.windows.net,1433;User ID=docs;Password=3h1@*6PXrldU4F95;Integrated Security=false;Initial Catalog=adafyweikiodevtestdb001;",
                        Tables = new[] {"Product*"},
                    })
                .AddOpenApi("/pets",
                    new ApiOptions
                    {
                        SpecificationUrl = "https://petstore.swagger.io/v2/swagger.json", 
                        IncludeHttpMethods = new[] { "GET" }
                    }
                )
                .AddOpenApi("/pets_writeonly",
                    new ApiOptions
                    {
                        SpecificationUrl = "https://petstore.swagger.io/v2/swagger.json", 
                        ExcludeHttpMethods = new[] { "GET" }
                    }
                )
                .AddEndpoint<CalculatorApi>("/mycal", 15)
                .AddEndpoint<CalculatorApi>("/second", 100)
                .AddEndpoint<CalculatorApi>("/third", 0);
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