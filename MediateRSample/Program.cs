using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediateRSample
{
    class Program
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
    /// Main Class
    /// </summary>
    public class Main
    {
        private readonly IMediator _mediator;
        public Main(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Start()
        {
            _mediator.Send(new Command { Name = "Manu" });
        }
    }

    /// <summary>
    /// Command
    /// </summary>
    public class Command : IRequest<object>
    {
        public string Name { get; set; }
        public string Age { get; set; }
    }

    /// <summary>
    /// Command Handler
    /// </summary>
    public class CommandHandler : IRequestHandler<Command, object>
    {
        private readonly A _a;
        public CommandHandler(A a = null)
        {
            _a = a;
        }
        public Task<object> Handle(Command request, CancellationToken cancellationToken)
        {
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(request));
            // Take Action on what to do with request
            object result = new { Message = "Successful" };
            return Task.FromResult(result);
        }
    }

    /// <summary>
    /// Boot Class
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
            
            Services.AddMediatR(typeof(StartUp));
            Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            Services.AddTransient<AbstractValidator<Command>, CommandValidator>();
            Services.AddTransient<AbstractValidator<Command>, CommandValidatorA>();
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

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Age).NotEmpty().WithMessage("Age must be specified");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please specify a first name");
        }
    }

    public class CommandValidatorA : AbstractValidator<Command>
    {
        public CommandValidatorA()
        {
            RuleFor(x => x.Age).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please specify a first name");
        }
    }

    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<AbstractValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<AbstractValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return next();
        }
    }

    public class A
    {
        public string Name { get; set; }
    }
}
