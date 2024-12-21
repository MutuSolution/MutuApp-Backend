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

    [HttpGet("all-core")]
    [MustHavePermission(AppFeature.Links, AppAction.Read)]
    public async Task<IActionResult> GetLinkList()
    {
        var response = await MediatorSender.Send(new GetLinksQuery());
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

    [HttpGet]
    [MustHavePermission(AppFeature.Links, AppAction.Read)]
    public async Task<IActionResult> GetLinks([FromQuery] PaginationParams paginationParams)
    {
        var query = new GetPagedLinksQuery { PaginationParams = paginationParams };
        var result = await MediatorSender.Send(query);

        // Header'lara pagination bilgilerini ekle
        Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
        Response.Headers.Append("X-Page", result.Page.ToString());
        Response.Headers.Append("X-Items-Per-Page", result.ItemsPerPage.ToString());
        Response.Headers.Append("X-Has-Previous-Page", 
            result.HasPreviousPage.ToString());
        Response.Headers.Append("X-Has-Next-Page", result.HasNextPage.ToString());

        return Ok(result.Items);
    }
}
