using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Weikio.ApiFramework.AspNetCore.StarterKit;

namespace Weikio.ApiFramework.Samples.NoConfiguration
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddApiFrameworkStarterKit(options =>
            {
                options.AutoResolveApis = true;
                options.AutoResolveEndpoints = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
