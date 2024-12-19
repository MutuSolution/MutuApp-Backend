using Application.Services.Identity;
using AutoMapper;
using Common.Authorization;
using Common.Requests.Identity;
using Common.Responses.Identity;
using Common.Responses.Wrappers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Identity;

public class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IResponseWrapper> CreateRoleAsync(CreateRoleRequest request)
    {
        var roleExist = await _roleManager.FindByNameAsync(request.RoleName);
        if (roleExist != null)
            return await ResponseWrapper<string>.FailAsync("Role already exists.");

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

    public async Task<IResponseWrapper> DeleteRoleAsync(string roleId)
    {
        var roleInDb = await _roleManager.FindByIdAsync(roleId);
        if (roleInDb is null) 
            return await ResponseWrapper.FailAsync("Role does not exist.");
        if (roleInDb.Name == AppRoles.Admin)
            return await ResponseWrapper.FailAsync("Role delete not permitted.");

        var allUsers = await _userManager.Users.ToListAsync();
        foreach (var user in allUsers)
        {
            if (await _userManager.IsInRoleAsync(user, roleInDb.Name))
                return await ResponseWrapper
                    .FailAsync($"Role: {roleInDb.Name} is currently assigned to a user.");
        }

        var identityResult = await _roleManager.DeleteAsync(roleInDb);
        if (!identityResult.Succeeded)
            return await ResponseWrapper<string>
                .FailAsync(GetIdentityResultErrorDescriptions(identityResult));

        return await ResponseWrapper<string>.SuccessAsync("Role deleted successfully");
    }

    public async Task<IResponseWrapper> GetRoleByIdAsync(string roleId)
    {
        var roleInDb = await _roleManager.FindByIdAsync(roleId);
        if (roleInDb is null)
            return await ResponseWrapper.FailAsync("No role was found.");

        var mappedRole = _mapper.Map<RoleResponse>(roleInDb);
        return await ResponseWrapper<RoleResponse>.SuccessAsync(mappedRole);
    }

    public async Task<IResponseWrapper> GetRolesAsync()
    {
        var allRoles = await _roleManager.Roles.ToListAsync();
        if (allRoles.Count < 1) 
            return await ResponseWrapper<string>.FailAsync("No roles were found.");

        var mappedRoles = _mapper.Map<List<RoleResponse>>(allRoles);
        return await ResponseWrapper<List<RoleResponse>>.SuccessAsync(mappedRoles);
    }

    public async Task<IResponseWrapper> UpdateRoleAsync(UpdateRoleRequest request)
    {
        var roleInDb = await _roleManager.FindByIdAsync(request.RoleId);
        if (roleInDb is null) 
            return await ResponseWrapper.FailAsync("Role does not exist");
        if (roleInDb.Name == AppRoles.Admin)
            return await ResponseWrapper.FailAsync("Role update not permitted.");

        roleInDb.Name = request.RoleName;
        roleInDb.Description = request.RoleDescription;

        var identityResult = await _roleManager.UpdateAsync(roleInDb);

        if (!identityResult.Succeeded)
            return await ResponseWrapper<string>
                .FailAsync(GetIdentityResultErrorDescriptions(identityResult));

        return await ResponseWrapper<string>.SuccessAsync("Role updated successfully");
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
