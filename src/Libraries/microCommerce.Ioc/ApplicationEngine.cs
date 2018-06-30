using Autofac;
using Autofac.Extensions.DependencyInjection;
using microCommerce.Common;
using microCommerce.Common.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace microCommerce.Ioc
{
    public class ApplicationEngine : IEngine
    {
        #region Fields
        private IServiceProvider _serviceProvider { get; set; }
        #endregion

        #region Properties
        public virtual IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider;
            }
        }
        #endregion

        public virtual IServiceProvider RegisterDependencies(IServiceCollection services, IConfiguration configuration, IAppConfiguration config)
        {
            var containerBuilder = new ContainerBuilder();

            //register engine
            containerBuilder.RegisterInstance(this).As<IEngine>().SingleInstance();

            //register assembly finder
            var assemblyHelper = new AssemblyHelper();
            containerBuilder.RegisterInstance(assemblyHelper).As<IAssemblyHelper>().SingleInstance();
            
            var dependencyConfig = new DependencyContext
            {
                ContainerBuilder = containerBuilder,
                AssemblyHelper = assemblyHelper,
                Configuration = configuration,
                AppConfig = config
            };

            //find dependency registrars provided by other assemblies
            var dependencies = assemblyHelper.FindOfType<IDependencyRegistrar>().ToList();

            //create instances of dependency registrars
            dependencies.ForEach(instance => CreateInstance<IDependencyRegistrar>(instance).Register(dependencyConfig));

            //populate Autofac container builder with the set of registered service descriptors
            containerBuilder.Populate(services);

            //create service provider
            _serviceProvider = new AutofacServiceProvider(containerBuilder.Build());

            //find startup tasks by other assemblies
            var startupTasks = assemblyHelper.FindOfType<IStartupTask>().ToList();

            //create instance of startup tasks
            startupTasks.ForEach(instance => ResolveUnregistered<IStartupTask>(instance).Execute());

            return _serviceProvider;
        }

        public virtual T CreateInstance<T>() where T : class
        {
            return CreateInstance(typeof(T)) as T;
        }

        public virtual T CreateInstance<T>(Type type) where T : class
        {
            return CreateInstance(type) as T;
        }

        public virtual object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Gets the instance by generic type from ioc container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Resolve<T>() where T : class
        {
            return Resolve(typeof(T)) as T;
        }

        /// <summary>
        /// Gets the instance by type from ioc container
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual object Resolve(Type type)
        {
            return ServiceProvider.GetRequiredService(type);
        }

        /// <summary>
        /// Gets the multiple instance by generic type from ioc container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IEnumerable<T> ResolveAll<T>()
        {
            return ServiceProvider.GetServices(typeof(T)) as IEnumerable<T>;
        }

        public virtual T ResolveUnregistered<T>(Type type) where T : class
        {
            return ResolveUnregistered(type) as T;
        }

        /// <summary>
        /// Gets the unregistered instance
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual object ResolveUnregistered(Type type)
        {
            Exception innerException = null;
            foreach (var constructor in type.GetConstructors())
            {
                try
                {
                    //try to resolve constructor parameters
                    var parameters = constructor.GetParameters().Select(parameter =>
                    {
                        var service = Resolve(parameter.ParameterType);
                        if (service == null)
                            throw new CustomException("Unknown dependency");

                        return service;
                    });

                    //all is ok, so create instance
                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }

            throw new CustomException("No constructor was found that had all the dependencies satisfied.", innerException);
        }
    }
}