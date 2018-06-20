using Autofac;
using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.Common.Configurations;
using microCommerce.Dapper;
using microCommerce.Dapper.Providers;
using microCommerce.Ioc;
using microCommerce.Logging;
using microCommerce.Mvc;
using microCommerce.Services.Media;
using System.Data;

namespace microCommerce.MediaApi.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(DependencyContext context)
        {
            var builder = context.ContainerBuilder;
            var config = context.AppConfig as ServiceConfiguration;

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
                //null logger
                builder.RegisterType<NullCacheManager>().As<ICacheManager>().SingleInstance();
            }

            //logging
            if (config.LoggingEnabled)
            {
                //default logger
                builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();
            }
            else
            {
                //null logger
                builder.RegisterType<NullLogger>().As<ILogger>().InstancePerLifetimeScope();
            }
            
            //dapper data context
            builder.Register(c => ProviderFactory.GetProvider(config.DatabaseProviderName)).As<IDataProvider>().SingleInstance();
            builder.Register(c => c.Resolve<IDataProvider>().CreateConnection(config.ConnectionString)).InstancePerLifetimeScope();
            builder.Register(c => new DataContext(c.Resolve<IDataProvider>(), c.Resolve<IDbConnection>())).As<IDataContext>().InstancePerLifetimeScope();

            //providers
            builder.RegisterType<PictureProvider>().As<IPictureProvider>().InstancePerLifetimeScope();
        }
    }
}