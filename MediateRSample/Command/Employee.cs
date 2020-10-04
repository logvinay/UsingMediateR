using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediateRSample.Command
{
    /// <summary>
    /// Command
    /// </summary>
    public class Employee : IRequest<object>
    {
        public Employee()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int NetSalary { get; set; }
    }
}
