using FluentValidation;
using MediateRSample.Behaviors;
using MediateRSample.Command;
using MediateRSample.Validators;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediateRSample
{
    class Boot
    {
        static void Main(string[] args)
        {
            // Registering required services into DI container;
            var serviceProvider = BuildServiceProvider<StartUp>();
            // resolving main class object from DI container;
            var main = serviceProvider.GetRequiredService<Main>();
            // Running Driver functions
            main.Start();
            Console.ReadLine();
        }

        /// <summary>
        /// Build Service Provider
        /// </summary>
        /// <typeparam name="T">StartUp Class</typeparam>
        /// <returns></returns>
        static ServiceProvider BuildServiceProvider<T>() where T : class
        {
            var startUpObj = Activator.CreateInstance(typeof(T));
            var configureService = typeof(T).GetMethod("ConfigureServices");
            var getServiceProvider = typeof(T).GetMethod("GetServiceProvider");
            var services = new ServiceCollection();
            object[] parames = { services };
            configureService.Invoke(startUpObj, parames);
            return (ServiceProvider)getServiceProvider.Invoke(startUpObj, new object[] { });
        }
    }


    /// <summary>
    /// Start Up
    /// </summary>
    public class StartUp
    {
        private IServiceCollection Services { get; set; }
        /// <summary>
        /// ctor
        /// </summary>
        public StartUp()
        {
            Services = new ServiceCollection();
        }

        /// <summary>
        /// Configure Services
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            if (services != null)
            {
                Services = services;
            }
            Services.AddMediatR(typeof(Boot));
            Services.AddTransient<AbstractValidator<Employee>, EmployeeValidator>();
            Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
            Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            Services.AddTransient<Main>();
        }


        /// <summary>
        /// Get Services Provider
        /// </summary>
        /// <returns></returns>
        public ServiceProvider GetServiceProvider()
        {
            return Services.BuildServiceProvider();
        }
    }
}
