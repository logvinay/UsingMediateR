using MediateRSample.Command;
using MediatR;
using System;
using Serializer = System.Text.Json.JsonSerializer;

namespace MediateRSample
{
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
           var result =  _mediator.Send(new Employee() { Name = "Manu", NetSalary = 30000 });
            Console.WriteLine(Serializer.Serialize(result));
        }
    }
}
