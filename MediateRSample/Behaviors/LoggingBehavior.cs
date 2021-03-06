﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Serializer = System.Text.Json.JsonSerializer;

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
            /// Logging object can be resolved from factory
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // Pre Logging
            Console.WriteLine("Prelog:\n" + Serializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true}));

            // Middleware execution
            var result = await next();

            // Post Logging
            Console.WriteLine("PostLog:\n" + Serializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
            return result;
        }
    }
}
