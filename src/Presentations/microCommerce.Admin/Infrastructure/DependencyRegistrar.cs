using Autofac;
using BiletKesfet.Common;
using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.Domain.Settings;
using microCommerce.Ioc;
using microCommerce.Logging;
using microCommerce.Module.Core;
using microCommerce.Mvc;
using microCommerce.Mvc.HttpProviders;
using microCommerce.Mvc.Infrastructure;
using microCommerce.Mvc.Themes;
using microCommerce.Mvc.UI;
using microCommerce.Providers.Localizations;

namespace microCommerce.Admin.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(DependencyContext context)
        {
            var builder = context.ContainerBuilder;
            var config = context.AppConfig;

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            if (config.CachingEnabled)
            {
                //cache manager
                builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();
                builder.RegisterType<MemoryCacheManager>().As<IStaticCacheManager>().SingleInstance();
            }
            else
            {
                builder.RegisterType<NullCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();
            }

            if (config.LoggingEnabled)
            {
                builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<NullLogger>().As<ILogger>().InstancePerLifetimeScope();
            }

            //head builder
            builder.RegisterType<HeadBuilder>().As<IHeadBuilder>().InstancePerLifetimeScope();

            //file provider
            builder.RegisterType<CustomFileProvider>().As<ICustomFileProvider>().InstancePerLifetimeScope();

            //theme provider
            builder.RegisterType<ThemeProvider>().As<IThemeProvider>().InstancePerLifetimeScope();

            //module features
            builder.RegisterType<ModuleProvider>().As<IModuleProvider>().InstancePerLifetimeScope();

            //user agent helper
            builder.RegisterType<UserAgentHelper>().As<IUserAgentHelper>().InstancePerLifetimeScope();

            //http client
            builder.RegisterType<StandardHttpClient>().As<IHttpClient>().InstancePerLifetimeScope();

            builder.RegisterType<LocalizationProvider>().As<ILocalizationProvider>().InstancePerLifetimeScope();

            //work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();

            builder.RegisterInstance(new StoreSettings());
        }

        public int Priority => 1;
    }
}