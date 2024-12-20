using Application.Features.Employees.Commands;
using FluentValidation;

namespace Application.Features.Employees.Validators;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.CreateEmployeeRequest)
            .SetValidator(new CreateEmployeeRequestValidator());
    }
}
