using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
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
    public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Excetion Behavior
        /// </summary>
        public ExceptionBehavior(IServiceScopeFactory factory)
        {

        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse result = (dynamic)new { };
            try
            {
                result = await next();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception From Exception Behavior Pipeline:\n" + JsonSerializer.Serialize(e, new JsonSerializerOptions { WriteIndented = true }));
            }
            return (TResponse)result;
        }
    }
}
