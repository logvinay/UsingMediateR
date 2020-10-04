using FluentValidation;
using MediateRSample.Command;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediateRSample.CommandHandler
{
    /// <summary>
    /// Command Handler
    /// </summary>
    public class EmployeeHandler : IRequestHandler<Employee, object>
    {

        private readonly IServiceScopeFactory _factory;

        public EmployeeHandler(IServiceScopeFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Handle Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<object> Handle(Employee request, CancellationToken cancellationToken)
        {
            // Take Action on what to do with request
            object result = new { Message = "Successful" };
            return Task.FromResult(result);
        }
    }
}
