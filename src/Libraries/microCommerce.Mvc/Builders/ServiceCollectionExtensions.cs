using microCommerce.Common.Configurations;
using microCommerce.Ioc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Net;

namespace microCommerce.Mvc.Builders
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider ConfigureApiServices(this IServiceCollection services, IConfigurationRoot configuration, IHostingEnvironment environment)
        {
            //add application configuration parameters
            var config = services.ConfigureStartupConfig<ServiceConfiguration>(configuration.GetSection("Service"));
            services.AddSingleton(config);

            //add hosting configuration parameters
            var hostingConfig = services.ConfigureStartupConfig<HostingConfiguration>(configuration.GetSection("Hosting"));
            hostingConfig.ApplicationRootPath = environment.ContentRootPath;
            hostingConfig.ContentRootPath = environment.WebRootPath;
            hostingConfig.ModulesRootPath = Path.Combine(environment.ContentRootPath, "Modules");
            services.AddSingleton(hostingConfig);
            
            //most of API providers require TLS 1.2 nowadays
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //add response compression
            services.AddGzipResponseCompression();

            //add mvc engine
            var builder = services.AddWebApi();
            
            //add accessor to HttpContext
            services.AddHttpContextAccessor();

            //add swagger
            services.AddCustomizedSwagger(config);

            //create, initialize and register dependencies
            var engine = IocContainer.Create();
            var serviceProvider = engine.RegisterDependencies(services, configuration, config);

            //add module features support
            builder.AddModuleFeatures(services);

            return serviceProvider;
        }

        private static IMvcBuilder AddWebApi(this IServiceCollection services)
        {
            var builder = services.AddMvcCore();

            //add formatter mapping
            builder.AddFormatterMappings();

            //add data annotations
            builder.AddDataAnnotations();

            //add json serializer
            builder.AddJsonFormatters();

            //add api explorer for swagger
            builder.AddApiExplorer();

            return new MvcBuilder(builder.Services, builder.PartManager);
        }

        private static IMvcBuilder AddWebApi(this IServiceCollection services, Action<MvcOptions> setupAction)
        {
            var builder = services.AddWebApi();
            builder.Services.Configure(setupAction);

            return builder;
        }
        
        private static void AddCustomizedSwagger(this IServiceCollection services, ServiceConfiguration config)
        {
            services.AddSwaggerGen(c =>
            {
                string currentVersion = string.Format("v{0}", config.CurrentVersion);
                c.SwaggerDoc(currentVersion, new Info
                {
                    Title = config.ApplicationName,
                    Version = currentVersion,
                    Description = config.ApplicationDescription,
                    Contact = new Contact
                    {
                        Email = "info@microcommerce.org",
                        Url = "https://github.com/fsefacan/microCommerce"
                    },
                    License = new License
                    {
                        Name = "MIT License",
                        Url = "https://github.com/fsefacan/microCommerce/blob/master/LICENSE"
                    }
                });
            });
        }
    }
}