﻿using microCommerce.Mvc.Builders;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace microCommerce.CustomerApi
{
    public class Startup
    {
        /// <summary>
        /// Gets the application configuration
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Gets the hosting environments
        /// </summary>
        public IHostingEnvironment Environment { get; }

        /// <summary>
        /// Startup constructure
        /// </summary>
        /// <param name="environment"></param>
        public Startup(IHostingEnvironment environment)
        {
            Environment = environment;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .Build();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.ConfigureApiServices(Configuration, Environment);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app)
        {
            app.ConfigureApiPipeline(Environment);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options => options.AddServerHeader = false)
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}