using Application.Services;
using AutoMapper;
using Common.Responses.Links;
using Common.Responses.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Links.Queries;

public class GetPagedLinksQuery : IRequest<PaginationResult<LinkResponse>>
{
    public LinkParameters Parameters { get; set; } = new LinkParameters();
}

public class GetPagedLinksQueryHandler : IRequestHandler<GetPagedLinksQuery, PaginationResult<LinkResponse>>
{
    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;

    public GetPagedLinksQueryHandler(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }

    public async Task<PaginationResult<LinkResponse>> Handle(GetPagedLinksQuery request, CancellationToken cancellationToken)
    {
        var pagedResult = await _linkService.GetPagedLinksAsync(request.Parameters);
        var mappedItems = _mapper.Map<IEnumerable<LinkResponse>>(pagedResult.Items);

        return new PaginationResult<LinkResponse>(
            mappedItems,
            pagedResult.TotalCount,
            pagedResult.Page,
            pagedResult.ItemsPerPage

        );
    }
}
