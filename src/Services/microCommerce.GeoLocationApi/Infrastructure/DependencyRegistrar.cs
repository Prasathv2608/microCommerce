using Autofac;
using microCommerce.Caching;
using microCommerce.GeoLocationApi.Services;
using microCommerce.Ioc;
using microCommerce.Logging;

namespace microCommerce.GeoLocationApi.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(DependencyContext context)
        {
            var builder = context.ContainerBuilder;
            var config = context.AppConfig;
            
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
            
            //register services
            builder.RegisterType<MaxMindProvider>().As<ILocationProvider>().InstancePerLifetimeScope();
        }
    }
}