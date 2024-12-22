using Application.Features.Links.Commands;
using Application.Services;
using FluentValidation;

namespace Application.Features.Links.Validators;

public class UpdateLinkCommandValidator : AbstractValidator<UpdateLinkCommand>
{
    public UpdateLinkCommandValidator(ILinkService linkService)
    {
        RuleFor(x => x.UpdateLinkRequest)
            .SetValidator(new UpdateLinkRequestValidator(linkService));
    }
}
