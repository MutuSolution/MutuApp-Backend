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

    [HttpGet]
    [MustHavePermission(AppFeature.Links, AppAction.Read)]
    public async Task<IActionResult> GetLinks([FromQuery] LinkParameters parameters)
    {
        var query = new GetPagedLinksQuery { Parameters = parameters };
        var result = await MediatorSender.Send(query);
        if (result.IsSuccessful) return Ok(result);
        return NotFound(result);
    }

}









//headerde değer gönderme.
// Pagination bilgilerini JSON formatında oluştur
//var totalPages = result.ResponseData.TotalCount > 0 ? (int)Math
//    .Ceiling((double)result.ResponseData.TotalCount / result.ResponseData.ItemsPerPage) : 0;
//var paginationHeader = new
//{
//    current_page = result.ResponseData.Page,
//    items_per_page = result.ResponseData.ItemsPerPage,
//    total_items = result.ResponseData.TotalCount,
//    total_pages = totalPages,
//    has_previous_page = result.ResponseData.HasPreviousPage,
//    has_next_page = result.ResponseData.HasNextPage
//};
// Header'a ekle
//Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationHeader));
// return Ok(result.ResponseData.Items);