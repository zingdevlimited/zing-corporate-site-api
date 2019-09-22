// <copyright file="Startup.cs" company="ProspectSoft Ltd T/A Zing">
// Copyright (c) ProspectSoft Ltd T/A Zing. All rights reserved.
// </copyright>

using CorpSiteFunctions.Configuration;
using CorpSiteFunctions.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(CorpSiteFunctions.Startup))]

namespace CorpSiteFunctions
{
    /// <summary>
    /// Implementation of the FunctionsStartup.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// Configures the services for dependency injection
        /// </summary>
        /// <param name="builder"><see cref="IFunctionsHostBuilder"/>.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            services.AddLogging(options =>
            {
                options.AddApplicationInsights();
                options.AddFilter("CorpSiteFunctions", LogLevel.Information);
            });

            services.AddHttpClient();
            services.AddOptions<HubspotOptions>().Configure<IConfiguration>((settings, configuration) => configuration.GetSection("HubspotOptions").Bind(settings));
            services.AddOptions<RecaptchaOptions>().Configure<IConfiguration>((settings, configuration) => configuration.GetSection("RecaptchaOptions").Bind(settings));

            services.AddSingleton<IHubspotService, HubspotService>();
            services.AddSingleton<IRecaptchaService, RecaptchaService>();
        }
    }
}
