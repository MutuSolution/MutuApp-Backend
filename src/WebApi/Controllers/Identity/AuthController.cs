using Application.Features.Identity.Token.Queries;
using Application.Features.Identity.Users.Commands;
using Common.Requests.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Identity;

[Route("api/[controller]")]
public class AuthController : MyBaseController<AuthController>
{

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequest tokenRequest)
    {

        var response = await MediatorSender.Send(new GetTokenQuery { TokenRequest = tokenRequest });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> GetRefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        var response = await MediatorSender.Send(
            new GetRefreshTokenQuery { RefreshTokenRequest = refreshTokenRequest });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequest userRegistration)
    {
        var response = await MediatorSender
            .Send(new UserRegistrationCommand { UserRegistration = userRegistration });
        if (response.IsSuccessful) return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await Task.CompletedTask;
        return Ok();
    }
}
