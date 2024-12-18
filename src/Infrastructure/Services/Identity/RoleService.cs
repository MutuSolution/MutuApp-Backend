using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Wrappers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Identity;

public class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IResponseWrapper> CreateRoleAsync(CreateRoleRequest request)
    {
        var roleExist = await _roleManager.FindByNameAsync(request.RoleName);
        if (roleExist != null) return await ResponseWrapper<string>.FailAsync("Role already exists.");

        var newRole = new ApplicationRole
        {
            Name = request.RoleName,
            Description = request.RoleDescription
        };

        var identityResult = await _roleManager.CreateAsync(newRole);
        if (!identityResult.Succeeded)
            return await ResponseWrapper<string>
                .FailAsync(GetIdentityResultErrorDescriptions(identityResult));

        return await ResponseWrapper<string>.SuccessAsync("Role created successfully");
    }
    private List<string> GetIdentityResultErrorDescriptions(IdentityResult identityResult)
    {
        var errorDescriptions = new List<string>();
        foreach (var error in identityResult.Errors)
        {
            errorDescriptions.Add(error.Description);
        }
        return errorDescriptions;
    }
}
