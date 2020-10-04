using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediateRSample.Behaviors
{
    /// <summary>
    /// To Log the requestion
    /// Implementaion of IPipeline Behavior
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="factory"></param>
        public LoggingBehavior(IServiceScopeFactory factory)
        {

        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(request));
            return next();
        }
    }
}
