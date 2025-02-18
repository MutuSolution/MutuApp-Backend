using Application.Features.Links.Commands;
using Application.Features.Links.Commands.Report;
using Application.Features.Links.Queries;
using Application.Features.Links.Queries.Report;
using Common.Authorization;
using Common.Requests.Links;
using Common.Requests.Links.Report;
using Common.Responses.Pagination;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/[controller]")]

public class ReportController : MyBaseController<ReportController>
{
    [HttpPost("link")]
    [MustHavePermission(AppFeature.Links, AppAction.Read)]
    public async Task<IActionResult> LinkReport([FromBody] LinkReportRequest request)
    {
        var response = await MediatorSender
            .Send(new ReportLinkCommand { LinkReportRequest = request });
        if (response.IsSuccessful) return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("link")]
    [MustHavePermission(AppFeature.Links, AppAction.Read)]
    public async Task<IActionResult> GetLinkReportList()
    {
        var response = await MediatorSender.Send(new GetReportLinksQuery());
        if (response.IsSuccessful) return Ok(response);
        return NotFound(response);
    }

    [HttpPut("link")]
    [MustHavePermission(AppFeature.Links, AppAction.Update)]
    public async Task<IActionResult> UpdateLinkReport([FromBody] LinkReportIsCheckedRequest request)
    {
        var response = await MediatorSender
            .Send(new UpdateReportLinkCommand { ReportId = request.ReportId, IsChecked = request.IsChecked });
        if (response.IsSuccessful) return Ok(response);
        return BadRequest(response);
    }

}
 