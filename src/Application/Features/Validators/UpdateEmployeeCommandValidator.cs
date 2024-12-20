using Application.Features.Employees.Commands;
using Application.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Validators;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator(IEmployeeService employeeService)
    {
        RuleFor(x => x.UpdateEmployeeRequest)
            .SetValidator(new UpdateEmployeeRequestValidator(employeeService));
    }
}
