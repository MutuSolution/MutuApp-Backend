namespace WebApi.Controllers;

using Application.Features.Identity.Users.Commands;
using Application.Features.Links.Commands;
using Application.Features.Links.Queries;
using Common.Authorization;
using Common.Requests.Identity;
using Common.Requests.Links;
using Common.Responses.Pagination;
using global::WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Attributes;

[Route("api/[controller]")]
public class LikesController : MyBaseController<LikesController>
{

    [HttpPost]
    [MustHavePermission(AppFeature.Links, AppAction.Update)]
    public async Task<IActionResult> DoLikeAsync([FromBody] LikeLinkRequest request)
    {
        var response = await MediatorSender.Send(new LikeCommand { LikeRequest = request });
        if (response.IsSuccessful) return Ok(response);
        return NotFound(response);
    }


    [HttpGet("isLike/{linkId:int}")]
    [MustHavePermission(AppFeature.Links, AppAction.Read)]
    public async Task<IActionResult> IsLikeAsync(int linkId)
    {
        var response = await MediatorSender.Send(new IsLikedQuery { LinkId = linkId });
        if (response.IsSuccessful) return Ok(response);
        return NotFound(response);
    }

    [HttpGet("one-user")]
    [MustHavePermission(AppFeature.Links, AppAction.Read)]
    public async Task<IActionResult> GetLikesByUserNameAsync([FromQuery] LikesByUserNameParameters parameters)
    {
        var query = new GetPagedLikesByUserNameQuery { Parameters = parameters };
        var result = await MediatorSender.Send(query);
        if (result.IsSuccessful) return Ok(result);
        return NotFound(result);
    }
}