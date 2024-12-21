using Application.Services;
using AutoMapper;
using Common.Responses.Links;
using Common.Responses.Pagination;
using Common.Responses.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Links.Queries;

public class GetPagedLinksQuery : IRequest<IResponseWrapper<PaginationResult<LinkResponse>>>
{
    public LinkParameters Parameters { get; set; } = new LinkParameters();
}

public class GetPagedLinksQueryHandler : IRequestHandler<GetPagedLinksQuery, IResponseWrapper<PaginationResult<LinkResponse>>>
{
    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;

    public GetPagedLinksQueryHandler(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }
  
    async Task<IResponseWrapper<PaginationResult<LinkResponse>>> IRequestHandler<GetPagedLinksQuery, IResponseWrapper<PaginationResult<LinkResponse>>>.Handle(GetPagedLinksQuery request, CancellationToken cancellationToken)
    {
        var pagedResult = await _linkService.GetPagedLinksAsync(request.Parameters);
        var mappedItems = _mapper.Map<IEnumerable<LinkResponse>>(pagedResult.Items);
        return await ResponseWrapper<PaginationResult<LinkResponse>>
        .SuccessAsync(new PaginationResult<LinkResponse>(
            mappedItems,
            pagedResult.TotalCount,
            pagedResult.TotalPage,
            pagedResult.Page,
            pagedResult.ItemsPerPage
        ));

    }
}
