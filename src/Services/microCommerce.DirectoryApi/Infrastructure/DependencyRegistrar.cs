using Autofac;
using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.Common.Configurations;
using microCommerce.Dapper;
using microCommerce.Dapper.Providers;
using microCommerce.Ioc;
using microCommerce.Logging;
using microCommerce.Mvc;
using microCommerce.Mvc.Infrastructure;
using microCommerce.Providers.Localizations;
using System.Data;

namespace microCommerce.DirectoryApi.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(DependencyContext context)
        {
            var builder = context.ContainerBuilder;
            var config = context.AppConfig as ServiceConfiguration;

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            //user agent helper
            builder.RegisterType<UserAgentHelper>().As<IUserAgentHelper>().InstancePerLifetimeScope();

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

            //dapper data context
            builder.Register(c => ProviderFactory.GetProvider(config.DatabaseProviderName)).As<IDataProvider>().SingleInstance();
            builder.Register(c => c.Resolve<IDataProvider>().CreateConnection(config.ConnectionString)).InstancePerLifetimeScope();
            builder.Register(c => new DataContext(c.Resolve<IDataProvider>(), c.Resolve<IDbConnection>())).As<IDataContext>().InstancePerLifetimeScope();

            //providers
            builder.RegisterType<LocalizationProvider>().As<ILocalizationProvider>().InstancePerLifetimeScope();

            //work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
        }

        public int Priority => 1;
    }
}