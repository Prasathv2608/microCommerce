using Hangfire;
using Hangfire.MemoryStorage;
using microCommerce.Common.Configurations;
using microCommerce.Ioc;
using microCommerce.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.WebEncoders;
using System;
using System.IO;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace microCommerce.Mvc.Builders
{
    public static class WebCollectionExtensions
    {
        public static IServiceProvider ConfigureServices(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment environment)
        {
            //add application configuration
            var config = services.ConfigureStartupConfig<WebConfiguration>(configuration.GetSection("Application"));
            services.AddSingleton(config);

            //add hosting configuration
            var hostingConfig = services.ConfigureStartupConfig<HostingConfiguration>(configuration.GetSection("Hosting"));
            hostingConfig.ApplicationRootPath = environment.ContentRootPath;
            hostingConfig.ContentRootPath = environment.WebRootPath;
            hostingConfig.ModulesRootPath = Path.Combine(environment.ContentRootPath, "Modules");
            services.AddSingleton(hostingConfig);

            //create, initialize and configure the engine
            var engine = EngineContext.Create();

            //most of API providers require TLS 1.2 nowadays
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //add non-latin character support
            services.AddTextEncoder();

            //add response compression
            services.AddGzipResponseCompression();

            //add mvc engine
            var builder = services.AddMvc();
            
            services.AddHangfire(hf => hf.UseMemoryStorage());
            
            //add custom view engine
            services.AddViewEngine();

            if (environment.ApplicationName.Equals("microCommerce.Web",
                StringComparison.InvariantCultureIgnoreCase))
            {
                //add theme support
                services.AddThemeSupport();
            }

            //add accessor to HttpContext
            services.AddHttpContextAccessor();

            //add anti forgery
            services.AddCustomAntiForgery();

            //add custom session
            services.AddCustomHttpSession(config);

            //register dependencies
            var serviceProvider = engine.RegisterDependencies(services, configuration, config);

            //add module features support
            builder.AddModuleFeatures(services);

            return serviceProvider;
        }

        private static void AddViewEngine(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.PageViewLocationFormats.Clear();
                options.AreaPageViewLocationFormats.Clear();

                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Areas/{2}/{Views}/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/{2}/{Views}/Shared/{0}.cshtml");

                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
                options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");

                options.ViewLocationExpanders.Clear();
                options.ViewLocationExpanders.Add(new BaseViewLocationExpander());
            });
        }

        private static void AddThemeSupport(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ThemeableViewLocationExpander());
            });
        }

        private static void AddTextEncoder(this IServiceCollection services)
        {
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });
        }

        /// <summary>
        /// Adds services required for anti-forgery support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        private static void AddCustomAntiForgery(this IServiceCollection services)
        {
            //override cookie name
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = ".microCommerce.Antiforgery";
            });
        }

        /// <summary>
        /// Adds services required for application session state
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        private static void AddCustomHttpSession(this IServiceCollection services, WebConfiguration config)
        {
            if (config.UseRedisSession)
            {
                services.AddDistributedRedisCache(options =>
                {
                    options.InstanceName = string.Empty;
                    options.Configuration = config.RedisConnectionString;
                });
            }

            services.AddSession(options =>
            {
                options.Cookie.Name = ".microCommerce.Session";
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });
        }
    }
}