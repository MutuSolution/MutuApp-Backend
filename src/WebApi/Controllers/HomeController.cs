
using Application.Features.Links.Queries;
using Application.Features.Links.Queries.Home;
using Common.Authorization;
using Common.Responses.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/[controller]")]
public class HomeController : MyBaseController<HomeController>
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetHomeLinkListAsync()
    {
        var response = await MediatorSender.Send(new GetHomeLinkQuery());
        if (response.IsSuccessful) return Ok(response);
        return NotFound(response);
    }

    [HttpGet("username/{userName:required}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicLinkWithUsernameListAsync(string userName)
    {
        var response = await MediatorSender.Send(new GetPublicLinkWithUsernameQuery { UserName = userName});
        if (response.IsSuccessful) return Ok(response);
        return NotFound(response);
    }
}
