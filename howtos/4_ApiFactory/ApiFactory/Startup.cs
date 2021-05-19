using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Weikio.ApiFramework.AspNetCore;
using Weikio.ApiFramework.AspNetCore.StarterKit;
using Weikio.ApiFramework.Core.Extensions;

namespace ApiFactory
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

            services.AddApiFrameworkStarterKit()
                .AddApi<CalcApiFactory>()
                .AddEndpoint("/sumcalc", "ApiFactory.CalcApiFactory", new CalcConfiguration()
                {
                    CanSum = true, 
                    CanMinus = false, 
                    CanMultiply = false
                })
                .AddEndpoint("/sumandmultiplecalc", "ApiFactory.CalcApiFactory", new CalcConfiguration() 
                { 
                    CanSum = true, 
                    CanMinus = false, 
                    CanMultiply = true 
                });
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

    public class SumOp
    {
        public int Sum(int x, int y)
        {
            return x + y;
        }
    }
    
    public class MinusOp
    {
        public int Minus(int x, int y)
        {
            return x - y;
        }
    }

    public class MultiplyOp
    {
        public int Multiply(int x, int y)
        {
            return x * y;
        }
    }

    public class CalcConfiguration
    {
        public bool CanSum { get; set; } = true;
        public bool CanMinus { get; set; } = true;
        public bool CanMultiply { get; set; } = true;
    }

    public class CalcApiFactory
    {
        public List<Type> Create(CalcConfiguration configuration)
        {
            var result = new List<Type>();

            if (configuration.CanSum)
            {
                result.Add(typeof(SumOp));
            }

            if (configuration.CanMinus)
            {
                result.Add(typeof(MinusOp));
            }
            
            if (configuration.CanMultiply)
            {
                result.Add(typeof(MultiplyOp));
            }

            return result;
        }
    }
}
