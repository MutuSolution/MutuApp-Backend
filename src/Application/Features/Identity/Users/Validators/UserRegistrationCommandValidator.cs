﻿using Application.Features.Identity.Users.Commands;
using Application.Services.Identity;
using FluentValidation;

namespace Application.Features.Identity.Users.Validators;

public class UserRegistrationCommandValidator : AbstractValidator<UserRegistrationCommand>
{
    public UserRegistrationCommandValidator(IUserService userService)
    {
        RuleFor(x => x.UserRegistration)
            .SetValidator(new UserRegistrationRequestValidator(userService));
    }
}
