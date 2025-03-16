using Application.Services;
using AutoMapper;
using Common.Responses.Links;
using Common.Responses.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Links.Queries.Report;

public class GetReportLinksQuery : IRequest<IResponseWrapper>
{
}

public class GetReportLinksQueryHandler : IRequestHandler<GetReportLinksQuery, IResponseWrapper>
{
    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;

    public GetReportLinksQueryHandler(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }
    public async Task<IResponseWrapper> Handle(GetReportLinksQuery request, CancellationToken cancellationToken)
    {
        var linkList = await _linkService.GetLinkReportsAsync();
        var mappedList = _mapper.Map<List<LinkReportResponse>>(linkList);

        return await Task.FromResult(
            ResponseWrapper<List<LinkReportResponse>>.Success(mappedList)
        );
    }
}