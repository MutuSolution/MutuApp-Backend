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
        return Ok(response);
    }

    [HttpPut("link")]
    [MustHavePermission(AppFeature.Links, AppAction.Update)]
    public async Task<IActionResult> UpdateLinkReport([FromBody] LinkReporIsPermittedRequest request)
    {
        var response = await MediatorSender
            .Send(new UpdateReportLinkCommand { ReportId = request.ReportId, IsPermitted = request.IsPermitted });
        if (response.IsSuccessful) return Ok(response);
        return BadRequest(response);
    }

}
 