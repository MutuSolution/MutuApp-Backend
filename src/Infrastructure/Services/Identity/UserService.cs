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
    private readonly IMapper _mapper;

    public UserService(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
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
            return await ResponseWrapper.FailAsync("Failed changing password.");
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

        return await ResponseWrapper.FailAsync(request.Activate ?
                "Failed to activate user." : "Failed to de-activate user.");
    }

    public async Task<IResponseWrapper> GetAllUsersAsync()
    {
        var userInDb = await _userManager.Users.ToListAsync();
        if (userInDb.Count <= 0) return await ResponseWrapper.FailAsync("No users were found.");
        var mappedUsers = _mapper.Map<List<UserResponse>>(userInDb);
        return await ResponseWrapper<List<UserResponse>>.SuccessAsync(mappedUsers);
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
            return await ResponseWrapper.FailAsync("User registration failed.");
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

        return await ResponseWrapper.FailAsync("Failed to update user details.");
    }
}
