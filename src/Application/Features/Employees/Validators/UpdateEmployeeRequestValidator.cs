using Application.Services;
using Common.Requests.Employees;
using Domain;
using FluentValidation;

namespace Application.Features.Employees.Validators;

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator(IEmployeeService employeeService)
    {
        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellation) => await employeeService
            .GetEmployeeByIdAsync(id) is not Employee existing)
            .WithMessage("Employee does not exit.");

        RuleFor(x => x.FirstName)
        .NotEmpty()
        .MaximumLength(200);

        RuleFor(x => x.LastName)
        .NotEmpty()
        .MaximumLength(200);

        RuleFor(x => x.Email)
        .EmailAddress()
        .NotEmpty();
    }
}
