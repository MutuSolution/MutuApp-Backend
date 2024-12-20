using Application.Features.Links.Commands;
using FluentValidation;

namespace Application.Features.Links.Validators;

public class CreateLinkCommandValidator : AbstractValidator<CreateLinkCommand>
{
    public CreateLinkCommandValidator()
    {
        RuleFor(x => x.CreateLinkRequest)
            .SetValidator(new CreateLinkRequestValidator());
    }
}
