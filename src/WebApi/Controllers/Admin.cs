using Application.Services.Identity;
using AutoMapper;
using Common.Authorization;
using Common.Responses.Wrappers;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Admin : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public Admin(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ICurrentUserService currentUserService, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }



    // POST api/<Admin>
    public class UserEmailStateRequest
    {
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }

    [HttpPost("user-email-state")]
    [MustHavePermission(AppFeature.UserRoles, AppAction.Update)]
    public async Task<IResponseWrapper> UserEmailActive([FromBody] UserEmailStateRequest request)
    {
        var currentLoggedInUser = await _userManager.FindByNameAsync(request.UserName);
        if (currentLoggedInUser is null)
            return await ResponseWrapper.FailAsync("[ML102] User does not exist");

        currentLoggedInUser.EmailConfirmed = request.IsActive;

        var identityResult = await _userManager.UpdateAsync(currentLoggedInUser);
        if (identityResult.Succeeded)
            return await ResponseWrapper<string>
                .SuccessAsync("[ML103] User email state successfully updated.");
        return await ResponseWrapper
            .FailAsync(GetIdentityResultErrorDescriptions(identityResult));
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