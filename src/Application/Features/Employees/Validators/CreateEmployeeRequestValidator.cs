using Common.Requests.Employees;
using FluentValidation;

namespace Application.Features.Employees.Validators;

public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator()
    {
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
