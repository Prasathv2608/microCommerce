using microCommerce.Common.Configurations;
using microCommerce.Ioc;
using microCommerce.Module.Core;
using microCommerce.Mvc.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
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
        public static IServiceProvider ConfigureServices(this IServiceCollection services, IConfigurationRoot configuration, IHostingEnvironment environment)
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
            services.AddCustomResponseCompression();

            //add mvc engine
            var builder = services.AddMvc();

            //add custom view engine
            services.AddViewEngine();

            if (environment.ApplicationName.Equals("microCommerce.Web"))
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

        private static IMvcBuilder AddModuleFeatures(this IMvcBuilder builder, IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Clear();
                options.ViewLocationExpanders.Add(new ModuleViewLocationExpander());
            });

            builder.ConfigureApplicationPartManager(manager => ModuleManager.Initialize(manager));

            return builder;
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

        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        private static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        private static void AddTextEncoder(this IServiceCollection services)
        {
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });
        }

        private static void AddCustomResponseCompression(this IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
                options.MimeTypes = new[] {
                    // Default
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                    // Custom
                    "image/svg+xml"
                };
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

        /// <summary>
        /// Create, bind and register as service the specified configuration parameters
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        private static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            return config;
        }
    }
}