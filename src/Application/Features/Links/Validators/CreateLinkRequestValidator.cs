using Common.Requests.Links;
using FluentValidation;

namespace Application.Features.Links.Validators;

public class CreateLinkRequestValidator : AbstractValidator<CreateLinkRequest>
{
    public CreateLinkRequestValidator()
    {
        RuleFor(x => x.Title)
      .NotEmpty();

        RuleFor(x => x.Url)
        .NotEmpty();

        RuleFor(x => x.UserName)
        .NotEmpty();

        RuleFor(x => x.Description)
        .NotEmpty();

    }
}
