﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ChinaBeacon.Sdk.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChinaBeacon.Sdk.Core.Infrastructure
{
    public class Engine : IEngine
    {
        #region Properties

        /// <summary>
        /// Gets or sets service provider
        /// </summary>
        private IServiceProvider _serviceProvider { get; set; }

        #endregion

        #region Utilities

        protected IServiceProvider GetServiceProvider()
        {
            var accessor = ServiceProvider.GetService<IHttpContextAccessor>();
            var context = accessor.HttpContext;
            return context != null ? context.RequestServices : ServiceProvider;
        }

        /// <summary>
        /// Run startup tasks
        /// </summary>
        /// <param name="typeFinder">Type finder</param>
        protected virtual void RunStartupTasks(ITypeFinder typeFinder)
        {
            //find startup tasks provided by other assemblies
            var startupTasks = typeFinder.FindClassesOfType<IStartupTask>();

            //create and sort instances of startup tasks
            //we startup this interface even for not installed plugins. 
            //otherwise, DbContext initializers won't run and a plugin installation won't work
            var instances = startupTasks
                .Select(startupTask => (IStartupTask)Activator.CreateInstance(startupTask))
                .OrderBy(startupTask => startupTask.Order);

            //execute tasks
            foreach (var task in instances)
                task.Execute();
        }

        /// <summary>
        /// Register dependencies using Autofac
        /// </summary>
        /// <param name="chinaBeaconGenericBusinessConfig">Startup Nop configuration parameters</param>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="typeFinder">Type finder</param>
        protected virtual IServiceProvider RegisterDependencies(ChinaBeaconConfig chinaBeaconGenericBusinessConfig, IServiceCollection services, ITypeFinder typeFinder)
        {
            var containerBuilder = new ContainerBuilder();

            //register engine
            containerBuilder.RegisterInstance(this).As<IEngine>().SingleInstance();

            //register type finder
            containerBuilder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();

            //find dependency registrars provided by other assemblies
            var dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyRegistrar>();

            //create and sort instances of dependency registrars
            var instances = dependencyRegistrars
                //.Where(dependencyRegistrar => PluginManager.FindPlugin(dependencyRegistrar).Return(plugin => plugin.Installed, true)) //ignore not installed plugins
                .Select(dependencyRegistrar => (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);

            //register all provided dependencies
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.Register(containerBuilder, typeFinder, chinaBeaconGenericBusinessConfig);

            //populate Autofac container builder with the set of registered service descriptors
            containerBuilder.Populate(services);

            //create service provider
            _serviceProvider = new AutofacServiceProvider(containerBuilder.Build());
            return _serviceProvider;
        }

//        /// <summary>
//        /// Register and configure AutoMapper
//        /// </summary>
//        /// <param name="services">Collection of service descriptors</param>
//        /// <param name="typeFinder">Type finder</param>
//        protected virtual void AddAutoMapper(IServiceCollection services, ITypeFinder typeFinder)
//        {
//            //find mapper configurations provided by other assemblies
//            var mapperConfigurations = typeFinder.FindClassesOfType<IMapperProfile>();
//
//            //create and sort instances of mapper configurations
//            var instances = mapperConfigurations
//                .Where(mapperConfiguration => PluginManager.FindPlugin(mapperConfiguration)?.Installed ?? true) //ignore not installed plugins
//                .Select(mapperConfiguration => (IMapperProfile)Activator.CreateInstance(mapperConfiguration))
//                .OrderBy(mapperConfiguration => mapperConfiguration.Order);
//
//            //create AutoMapper configuration
//            var config = new MapperConfiguration(cfg => {
//                foreach (var instance in instances)
//                {
//                    cfg.AddProfile(instance.GetType());
//                }
//            });
//
//            //register AutoMapper
//            services.AddAutoMapper();
//
//            //register
//            AutoMapperConfiguration.Init(config);
//        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize engine
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public void Initialize(IServiceCollection services)
        {
            //most of API providers require TLS 1.2 nowadays
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //set base application path
            var provider = services.BuildServiceProvider();
            var hostingEnvironment = provider.GetRequiredService<IHostingEnvironment>();
            //var nopConfig = provider.GetRequiredService<ChinaBeaconGenericBusinessConfig>();
            CommonHelper.BaseDirectory = hostingEnvironment.ContentRootPath;

            //initialize plugins
//            var mvcCoreBuilder = services.AddMvcCore();
//            PluginManager.Initialize(mvcCoreBuilder.PartManager, nopConfig);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //check for assembly already loaded
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
                return assembly;

            //get assembly fron TypeFinder
            var tf = Resolve<ITypeFinder>();
            assembly = tf.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            return assembly;
        }

        /// <summary>
        /// Add and configure services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        /// <returns>Service provider</returns>
        public IServiceProvider ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            //find startup configurations provided by other assemblies
            var typeFinder = new WebAppTypeFinder();
           // var startupConfigurations = typeFinder.FindClassesOfType<INopStartup>();

//            //create and sort instances of startup configurations
//            var instances = startupConfigurations
//                .Where(startup => PluginManager.FindPlugin(startup)?.Installed ?? true) //ignore not installed plugins
//                .Select(startup => (INopStartup)Activator.CreateInstance(startup))
//                .OrderBy(startup => startup.Order);
//
//            //configure services
//            foreach (var instance in instances)
//                instance.ConfigureServices(services, configuration);

            //register mapper configurations
            //AddAutoMapper(services, typeFinder);

            //register dependencies
            var nopConfig = services.BuildServiceProvider().GetService<ChinaBeaconConfig>();
            RegisterDependencies(nopConfig, services, typeFinder);

            //run startup tasks
            if (!nopConfig.IgnoreStartupTasks)
                RunStartupTasks(typeFinder);

            //resolve assemblies here. otherwise, plugins can throw an exception when rendering views
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            //set App_Data path as base data directory (required to create and save SQL Server Compact database file in App_Data folder)
            AppDomain.CurrentDomain.SetData("DataDirectory", CommonHelper.MapPath("~/App_Data/"));

            return _serviceProvider;
        }

//        /// <summary>
//        /// Configure HTTP request pipeline
//        /// </summary>
//        /// <param name="application">Builder for configuring an application's request pipeline</param>
//        public void ConfigureRequestPipeline(IApplicationBuilder application)
//        {
//            //find startup configurations provided by other assemblies
//            var typeFinder = Resolve<ITypeFinder>();
//            var startupConfigurations = typeFinder.FindClassesOfType<INopStartup>();
//
//            //create and sort instances of startup configurations
//            var instances = startupConfigurations
//                .Where(startup => PluginManager.FindPlugin(startup)?.Installed ?? true) //ignore not installed plugins
//                .Select(startup => (INopStartup)Activator.CreateInstance(startup))
//                .OrderBy(startup => startup.Order);
//
//            //configure request pipeline
//            foreach (var instance in instances)
//                instance.Configure(application);
//        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">Type of resolved service</typeparam>
        /// <returns>Resolved service</returns>
        public T Resolve<T>() where T : class
        {
            return (T)GetServiceProvider().GetRequiredService(typeof(T));
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <param name="type">Type of resolved service</param>
        /// <returns>Resolved service</returns>
        public object Resolve(Type type)
        {
            return GetServiceProvider().GetRequiredService(type);
        }

        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">Type of resolved services</typeparam>
        /// <returns>Collection of resolved services</returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
        }

//        /// <summary>
//        /// Resolve unregistered service
//        /// </summary>
//        /// <param name="type">Type of service</param>
//        /// <returns>Resolved service</returns>
//        public virtual object ResolveUnregistered(Type type)
//        {
//            foreach (var constructor in type.GetConstructors())
//            {
//                try
//                {
//                    //try to resolve constructor parameters
//                    var parameters = constructor.GetParameters().Select(parameter =>
//                    {
//                        var service = Resolve(parameter.ParameterType);
//                        if (service == null)
//                            throw new NopException("Unknown dependency");
//                        return service;
//                    });
//
//                    //all is ok, so create instance
//                    return Activator.CreateInstance(type, parameters.ToArray());
//                }
//                catch (NopException) { }
//            }
//            throw new NopException("No constructor was found that had all the dependencies satisfied.");
//        }

        #endregion

        #region Properties

        /// <summary>
        /// Service provider
        /// </summary>
        public virtual IServiceProvider ServiceProvider => _serviceProvider;

        #endregion
    }
}
