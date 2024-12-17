using Application.Services.Identity;
using AutoMapper;
using Common.Authorization;
using Common.Requests.Identity;
using Common.Responses.Identity;
using Common.Responses.Wrappers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

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
}
