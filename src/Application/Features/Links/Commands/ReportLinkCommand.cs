using Application.Pipelines;
using Application.Services;
using AutoMapper;
using Common.Requests.Links;
using Common.Responses.Links;
using Common.Responses.Wrappers;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Links.Commands;

public class ReportLinkCommand : IRequest<IResponseWrapper>, IValidateMe
{
   public LinkReportRequest LinkReportRequest { get; set; }
}

public class ReportLinkCommandHandler :
    IRequestHandler<ReportLinkCommand, IResponseWrapper>
{

    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;

    public ReportLinkCommandHandler(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }
    public async Task<IResponseWrapper> Handle(ReportLinkCommand request, CancellationToken cancellationToken)
    {
        var mappedLinkReport = _mapper.Map<LinkReport>(request.LinkReportRequest);
        var newLinkReport = await _linkService.ReportLinkAsync(mappedLinkReport);
        if (newLinkReport.Id > 0)
        {
            var mappedNewLinkReport = _mapper.Map<LinkReportResponse>(newLinkReport);
            return await ResponseWrapper<LinkReportResponse>
                .SuccessAsync(mappedNewLinkReport, "[ML111] Link report created successfully.");
        }
        return await ResponseWrapper.FailAsync("[ML112] Failed to create link report entry.");
    }
}