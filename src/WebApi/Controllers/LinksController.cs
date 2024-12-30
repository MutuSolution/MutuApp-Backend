using Application.Features.Links.Commands;
using Application.Features.Links.Queries;
using Common.Authorization;
using Common.Requests.Links;
using Common.Responses.Pagination;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/[controller]")]
public class LinksController : MyBaseController<LinksController>
{
    [HttpPost]
    [MustHavePermission(AppFeature.Links, AppAction.Create)]
    public async Task<IActionResult> CreateLink([FromBody] CreateLinkRequest createLink)
    {
        var response = await MediatorSender
            .Send(new CreateLinkCommand { CreateLinkRequest = createLink });
        if (response.IsSuccessful) return Ok(response);
        return BadRequest(response);
    }

    [HttpPut]
    [MustHavePermission(AppFeature.Links, AppAction.Update)]
    public async Task<IActionResult> UpdateLink([FromBody] UpdateLinkRequest updateLink)
    {
        var response = await MediatorSender
            .Send(new UpdateLinkCommand { UpdateLinkRequest = updateLink });
        if (response.IsSuccessful) return Ok(response);
        return BadRequest(response);
    }

    [HttpDelete("{linkId:int}")]
    [MustHavePermission(AppFeature.Links, AppAction.Delete)]
    public async Task<IActionResult> DeleteLink(int linkId)
    {
        var response = await MediatorSender
            .Send(new DeleteLinkCommand { LinkId = linkId });
        if (response.IsSuccessful) return Ok(response);
        return NotFound(response);
    }

    [HttpGet("{linkId:int}")]
    [MustHavePermission(AppFeature.Links, AppAction.Read)]
    public async Task<IActionResult> GetLinkById(int linkId)
    {
        var response = await MediatorSender.Send(new GetLinkByIdQuery { LinkId = linkId });
        if (response.IsSuccessful) return Ok(response);
        return NotFound(response);
    }

    [HttpGet("all-core")]
    [MustHavePermission(AppFeature.Links, AppAction.Read)]
    public async Task<IActionResult> GetLinkList()
    {
        var response = await MediatorSender.Send(new GetLinksQuery());
        if (response.IsSuccessful) return Ok(response);
        return NotFound(response);
    }

    [HttpGet("all-one-user")]
    [MustHavePermission(AppFeature.Links, AppAction.Read)]
    public async Task<IActionResult> GetLinksByUserName([FromQuery] LinksByUserNameParameters parameters)
    {
        var query = new GetPagedLinksByUserNameQuery { Parameters = parameters };
        var result = await MediatorSender.Send(query);
        if (result.IsSuccessful) return Ok(result);
        return NotFound(result);
    }

    [HttpGet("all")]
    [MustHavePermission(AppFeature.Links, AppAction.Read)]
    public async Task<IActionResult> GetLinks([FromQuery] LinkParameters parameters)
    {
        var query = new GetPagedLinksQuery { Parameters = parameters };
        var result = await MediatorSender.Send(query);
        if (result.IsSuccessful) return Ok(result);
        return NotFound(result);
    }

    [HttpPut("soft-delete")]
    [MustHavePermission(AppFeature.Links, AppAction.Update)]
    public async Task<IActionResult> SoftDelete([FromBody] SoftDeleteLinkRequest request)
    {
        var response = await MediatorSender.Send(new SoftDeleteLinkCommand { SoftDeleteLinkRequest = request });
        if (response.IsSuccessful) return Ok(response);
        return NotFound(response);
    }
}