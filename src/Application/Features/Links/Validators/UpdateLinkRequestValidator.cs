using Application.Services;
using Common.Requests.Links;
using Domain;
using FluentValidation;

namespace Application.Features.Links.Validators;

public class UpdateLinkRequestValidator : AbstractValidator<UpdateLinkRequest>
{
    public UpdateLinkRequestValidator(ILinkService linkService)
    {

        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellation) => await linkService
            .GetLinkByIdAsync(id) is Link existing)
            .WithMessage("[ML_06] Link does not exit.");

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