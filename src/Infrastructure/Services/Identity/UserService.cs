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

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public UserService(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IResponseWrapper> ChangeUserPasswordAsync(ChangePasswordRequest request)
    {
        if (request.NewPassword != request.ConfirmedNewPassword)
            return ResponseWrapper.Fail("New passwords must be equal.");

        var userInDb = await _userManager.FindByIdAsync(request.UserId);
        if (userInDb == null) return await ResponseWrapper.FailAsync("User does not exist.");

        var identityResult = await _userManager
            .ChangePasswordAsync(userInDb, request.CurrentPassword, request.NewPassword);
        if (!identityResult.Succeeded)
            return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult));
        return await ResponseWrapper<string>.SuccessAsync("User password changed.");
    }

    public async Task<IResponseWrapper> ChangeUserStatusAsync(ChangeUserStatusRequest request)
    {
        var userInDb = await _userManager.FindByIdAsync(request.UserId);
        if (userInDb == null) return await ResponseWrapper.FailAsync("User does not exist.");

        userInDb.IsActive = request.Activate;
        var identityResult = await _userManager.UpdateAsync(userInDb);
        if (identityResult.Succeeded)
            return await ResponseWrapper<string>
                .SuccessAsync(request.Activate ?
                "User activated successfully." : "User de-activated successfully");

        return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult));
    }

    public async Task<IResponseWrapper> GetAllUsersAsync()
    {
        var userInDb = await _userManager.Users.ToListAsync();
        if (userInDb.Count <= 0) return await ResponseWrapper.FailAsync("No users were found.");
        var mappedUsers = _mapper.Map<List<UserResponse>>(userInDb);
        return await ResponseWrapper<List<UserResponse>>.SuccessAsync(mappedUsers);
    }

    public async Task<IResponseWrapper> GetRolesAsync(string userId)
    {
        var userRolesVM = new List<UserRoleViewModel>();

        var userInDb = await _userManager.FindByIdAsync(userId);
        if (userInDb == null) return await ResponseWrapper.FailAsync("User does not exist.");

        var allRoles = await _roleManager.Roles.ToListAsync();
        if (allRoles == null) return await ResponseWrapper.FailAsync("Role not found.");

        foreach (var role in allRoles)
        {
            var userRoleVM = new UserRoleViewModel
            {
                RoleName = role.Name
            };

            if (await _userManager.IsInRoleAsync(userInDb, role.Name))
            {
                userRoleVM.IsAssignedToUser = true;
            }
            else
            {
                userRoleVM.IsAssignedToUser = false;
            }
            userRolesVM.Add(userRoleVM);
        }
        return await ResponseWrapper<List<UserRoleViewModel>>.SuccessAsync(userRolesVM);
    }

    public async Task<IResponseWrapper<UserResponse>> GetUserByEmailAsync(string email)
    {
        var userInDb = await _userManager.FindByEmailAsync(email);
        if (userInDb == null)
            return await ResponseWrapper<UserResponse>.FailAsync("User not found.");

        var mappedUser = _mapper.Map<UserResponse>(userInDb);
        return await ResponseWrapper<UserResponse>.SuccessAsync(mappedUser);
    }

    public async Task<IResponseWrapper<UserResponse>> GetUserByUserNameAsync(string username)
    {
        var userInDb = await _userManager.FindByNameAsync(username);
        if (userInDb == null)
            return await ResponseWrapper<UserResponse>.FailAsync("User not found.");

        var mappedUser = _mapper.Map<UserResponse>(userInDb);
        return await ResponseWrapper<UserResponse>.SuccessAsync(mappedUser);
    }

    public async Task<IResponseWrapper> GetUserByIdAsync(string userId)
    {
        var userInDb = await _userManager.FindByIdAsync(userId);
        if (userInDb == null) return await ResponseWrapper.FailAsync("User not found.");

        var mappedUser = _mapper.Map<UserResponse>(userInDb);
        return await ResponseWrapper<UserResponse>.SuccessAsync(mappedUser);
    }

    public async Task<IResponseWrapper> RegisterUserAsync(UserRegistrationRequest request)
    {
        if (request.Password != request.ConfirmPassword)
            return ResponseWrapper.Fail("Password must be equal.");

        var userWithEmailInDb = await _userManager.FindByEmailAsync(request.Email);
        if (userWithEmailInDb is not null) await ResponseWrapper.FailAsync("Email already taken.");

        var userWithUserNameInDb = await _userManager.FindByNameAsync(request.UserName);
        if (userWithUserNameInDb is not null)
            await ResponseWrapper.FailAsync("Username already taken.");

        var newUser = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.UserName,
            IsActive = request.ActivateUser,
            EmailConfirmed = request.AutoConfirmEmail
        };
        var password = new PasswordHasher<ApplicationUser>();
        newUser.PasswordHash = password.HashPassword(newUser, request.Password);

        var identityResult = await _userManager.CreateAsync(newUser);

        if (identityResult.Succeeded)
        {
            //Assing user to basic role
            await _userManager.AddToRoleAsync(newUser, AppRoles.Basic);
            return await ResponseWrapper<string>.SuccessAsync("User registered successfully.");
        }
        else
        {
            return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult));
        }
    }

    public async Task<IResponseWrapper> UpdateUserAsync(UpdateUserRequest request)
    {
        var userInDb = await _userManager.FindByIdAsync(request.UserId);
        if (userInDb is null) return await ResponseWrapper.FailAsync("User does not exist.");

        userInDb.FirstName = request.FirstName;
        userInDb.LastName = request.LastName;

        var identityResult = await _userManager.UpdateAsync(userInDb);
        if (identityResult.Succeeded)
            return await ResponseWrapper<string>.SuccessAsync("User details successfully updated.");

        return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult));
    }

    public async Task<IResponseWrapper> UpdateUserRolesAsync(UpdateUserRolesRequest request)
    {
        var userInDb = await _userManager.FindByIdAsync(request.UserId);
        if (userInDb is not null)
        {
            if (userInDb.Email == AppCredentials.Email)
            {
                return await ResponseWrapper.FailAsync("User Roles update not permitted.");
            }
            var currentAssignedRoles = await _userManager.GetRolesAsync(userInDb);
            var rolesToBeAssigned = request.Roles
                .Where(role => role.IsAssignedToUser == true)
                .ToList();

            var currentLoggedInUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
            if (currentLoggedInUser is null)
            {
                return await ResponseWrapper.FailAsync("User does not exist.");
            }

            if (await _userManager.IsInRoleAsync(currentLoggedInUser, AppRoles.Admin))
            {
                var identityResult1 = await _userManager.RemoveFromRolesAsync(userInDb, currentAssignedRoles);
                if (identityResult1.Succeeded)
                {
                    var identityResult2 = await _userManager
                        .AddToRolesAsync(userInDb, rolesToBeAssigned.Select(role => role.RoleName));
                    if (identityResult2.Succeeded)
                    {
                        return await ResponseWrapper<string>.SuccessAsync("User Roles Updated Successfully.");
                    }
                    return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult2));
                }
                return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult1));
            }
            return await ResponseWrapper.FailAsync("User Roles update not permitted.");
        }
        return await ResponseWrapper.FailAsync("User does not exist.");
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
