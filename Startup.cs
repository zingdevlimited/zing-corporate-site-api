using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using corporate_site_api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace corporate_site_api
{
    public class Startup
    {
        private readonly ILogger logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            this.logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
                });
            });


            services.AddHttpClient();
            services.Configure<HubspotOptions>(Configuration.GetSection("HubspotOptions"));
            services.Configure<RecaptchaOptions>(Configuration.GetSection("RecaptchaOptions"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<IHubspotService,HubspotService>();
            
            services.AddScoped<IRecaptchaService,RecaptchaService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp => {
                    errorApp.Run(async context => {
                        var exceptionHandlerPathFeature = 
                        context.Features.Get<IExceptionHandlerPathFeature>();
                        Exception e = exceptionHandlerPathFeature?.Error;
                        logger.LogError(e.Message);
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An internal server error has occured.");
                    }

                    );}
                );
            }

            app.UseCors("EnableCORS");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
