using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MediateRSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider<StartUp>();
            var driverObj = serviceProvider.GetRequiredService<Driver>();
            driverObj.Run();
        }

        static ServiceProvider BuildServiceProvider<T>() where T : class
        {
            var startUpObj = Activator.CreateInstance(typeof(T));
            var configureService = typeof(T).GetMethod("ConfigureServices");
            var getServiceProvider = typeof(T).GetMethod("GetServiceProvider");
            var services = new ServiceCollection();
            object[] objects = { services, 243 };
            configureService.Invoke(startUpObj, objects);
            object[] obj = { };
            return (ServiceProvider)getServiceProvider.Invoke(startUpObj, obj);
        }
    }

    public class Driver
    {
        private readonly IMediator _mediator;
        public Driver(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Run()
        {
            _mediator.Send(new CommandA { Name = "Vinay", Age = "50" });
        }
    }

    public class CommandA : IRequest<object>
    {
        public string Name { get; set; }
        public string Age { get; set; }
    }

    public class CommandAHandler : IRequestHandler<CommandA, object>
    {
        public Task<object> Handle(CommandA request, CancellationToken cancellationToken)
        {
            Console.WriteLine(request.Name + " " + request.Age);
            return Task.FromResult(new object());
        }
    }

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
        public void ConfigureServices(IServiceCollection services, int abc)
        {
            if (services != null)
            {
                Services = services;
            }
            Services.AddMediatR(typeof(StartUp));
            Services.AddTransient<Driver>();
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
