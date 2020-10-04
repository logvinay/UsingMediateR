using FluentValidation;
using MediateRSample.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediateRSample.Validators
{
    /// <summary>
    /// Command Validator Based on abstraction of Fluent Validator
    /// </summary>
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Guid cannot be null");
            RuleFor(x => x.Age).NotEmpty().WithMessage("Age must be specified");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please specify a first name");
            RuleFor(x => x.NetSalary).NotEmpty().WithMessage("Net salary cannot be null");
        }
    }
}
