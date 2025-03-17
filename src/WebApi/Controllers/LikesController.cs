namespace WebApi.Controllers;

using Application.Features.Links.Commands;
using Application.Features.Links.Queries;
using Common.Authorization;
using Common.Requests.Links;
using Common.Responses.Pagination;
using global::WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

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

 

    [HttpGet("one-user")]
    [MustHavePermission(AppFeature.Links, AppAction.Read)]
    public async Task<IActionResult> GetLikesByUserNameAsync([FromQuery] LikesByUserNameParameters parameters)
    {
        var query = new GetPagedLikesByUserNameQuery { Parameters = parameters };
        var result = await MediatorSender.Send(query);
        return Ok(result);
    }
}