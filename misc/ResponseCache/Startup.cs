using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Weikio.ApiFramework.AspNetCore.StarterKit;
using Weikio.ApiFramework.ResponceCache;

namespace Weikio.ApiFramework.Samples.ResponseCache
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCaching();

            services.AddControllers();

            // Api Framework endpoint caching configuration is defined in the apiframework.json
            services.AddApiFrameworkStarterKit(options => options.AutoResolveApis = true)
                .AddApiFrameworkResponseCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseResponseCaching();
            app.UseApiFrameworkResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
