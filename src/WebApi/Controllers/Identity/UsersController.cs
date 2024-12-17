using Application.Features.Identity.Commands;
using Application.Features.Identity.Queries;
using Common.Requests.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Identity;

[Route("api/[controller]")]
public class UsersController : MyBaseController<UsersController>
{
    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequest userRegistration)
    {
        var response = await MediatorSender
            .Send(new UserRegistrationCommand { UserRegistration = userRegistration });
        if (response.IsSuccessful) return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var response = await MediatorSender
            .Send(new GetUserByIdQuery { UserId = userId });
        if (response.IsSuccessful) return Ok(response);
        return NotFound(response);
    }

}
