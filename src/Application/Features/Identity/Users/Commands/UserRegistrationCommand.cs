﻿using Application.Pipelines;
using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Users.Commands;

public class UserRegistrationCommand : IRequest<IResponseWrapper>, IValidateMe
{
    public UserRegistrationRequest UserRegistration { get; set; }
}

public class UserRegistrationCommandHandler :
    IRequestHandler<UserRegistrationCommand, IResponseWrapper>
{
    private readonly IUserService _userService;

    public UserRegistrationCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IResponseWrapper> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
    {
        return await _userService.RegisterUserAsync(request.UserRegistration);
    }
}
