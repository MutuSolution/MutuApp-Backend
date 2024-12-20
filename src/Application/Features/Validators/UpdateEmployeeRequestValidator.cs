using Application.Services;
using Common.Requests.Employees;
using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Validators;

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator(IEmployeeService employeeService)
    {
        RuleFor(x => x.Id)
            .MustAsync(async (id, y) => await employeeService.GetEmployeeByIdAsync(id)
            is Employee employeInDb && employeInDb.Id == id)
            .WithMessage("Employee does not exit.");

        RuleFor(x => x.FirstName)
        .NotEmpty()
        .MaximumLength(200);

        RuleFor(x => x.LastName)
        .NotEmpty()
        .MaximumLength(200);

        RuleFor(x => x.Email)
        .EmailAddress()
        .NotEmpty()
        .WithMessage("Employee email is required.");


    }
}
