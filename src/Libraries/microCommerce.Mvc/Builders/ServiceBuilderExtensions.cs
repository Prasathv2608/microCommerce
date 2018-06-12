using microCommerce.Common;
using microCommerce.Common.Configurations;
using microCommerce.Ioc;
using microCommerce.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace microCommerce.Mvc.Builders
{
    public static class ServiceBuilderExtensions
    {
        public static void ConfigureApiPipeline(this IApplicationBuilder app, IHostingEnvironment env)
        {
            //exception handling
            app.UseCustomExceptionHandler();

            //use gzip compression
            app.UseResponseCompression();

            app.UseMvc();
            app.UseStaticFiles();

            app.UseCustomizedSwagger();
        }
        
        private static void UseCustomizedSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(s =>
            {
                s.RouteTemplate = "docs/{documentName}/endpoints.json";
            });

            var config = EngineContext.Current.Resolve<ServiceConfiguration>();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint(string.Format("/docs/v{0}/endpoints.json", config.CurrentVersion), config.ApplicationName);
                s.RoutePrefix = "docs";
                s.DocumentTitle = config.ApplicationName;
            });
        }

        private static void UseCustomExceptionHandler(this IApplicationBuilder application)
        {
            var hostingEnvironment = EngineContext.Current.Resolve<IHostingEnvironment>();
            if (hostingEnvironment.IsDevelopment())
            {
                //get detailed exceptions for developing and testing purposes
                application.UseDeveloperExceptionPage();
            }

            //log errors
            application.UseExceptionHandler(handler =>
            {
                handler.Run(context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (exception == null)
                        return Task.CompletedTask;

                    try
                    {
                        var logger = EngineContext.Current.Resolve<ILogger>();
                        var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                        logger.Error(exception.Message, exception, webHelper.GetCurrentIpAddress(), webHelper.GetThisPageUrl(true), webHelper.GetUrlReferrer());
                    }
                    finally
                    {
                        //rethrow the exception to show the error page
                        throw exception;
                    }
                });
            });
        }
    }
}