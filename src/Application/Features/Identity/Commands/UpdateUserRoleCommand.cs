using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Commands;

public class UpdateUserRoleCommand : IRequest<IResponseWrapper>
{
    public UpdateUserRolesRequest UpdateUserRoles { get; set; }
}

public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, IResponseWrapper>
{
    private readonly IUserService _userService;

    public UpdateUserRoleCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IResponseWrapper> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        return await _userService.UpdateUserRolesAsync(request.UpdateUserRoles);
    }
}
