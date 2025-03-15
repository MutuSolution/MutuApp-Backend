using Application.Services;
using AutoMapper;
using Common.Requests.Links.Report;
using Common.Responses.Links;
using Common.Responses.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Links.Commands.Report;
public class UpdateReportLinkCommand : IRequest<IResponseWrapper>
{
    public int ReportId { get; set; }
    public bool IsChecked { get; set; }
}
public class UpdateReportLinkCommandHandler : IRequestHandler<UpdateReportLinkCommand, IResponseWrapper>
{
    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;
    public UpdateReportLinkCommandHandler(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }
    public async Task<IResponseWrapper> Handle(UpdateReportLinkCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = new LinkReportIsCheckedRequest
        {
            ReportId = request.ReportId,
            IsChecked = request.IsChecked
        };

        var updatedLink = await _linkService.UpdateReportLinkAsync(updateRequest);

        if (updatedLink.LinkId > 0)
        {
            var mappedUpdatedLink = _mapper.Map<LinkReportResponse>(updatedLink);
            return ResponseWrapper<LinkReportResponse>
                .Success(mappedUpdatedLink, "[ML114] Link report updated successfully.");
        }

        return ResponseWrapper.Fail("[ML115a] Failed to update link report entry.");
    }

}
