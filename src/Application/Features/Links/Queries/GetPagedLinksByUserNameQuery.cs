using Application.Services;
using AutoMapper;
using Common.Responses.Links;
using Common.Responses.Pagination;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Links.Queries;

public class GetPagedLinksByUserNameQuery : IRequest<IResponseWrapper<PaginationResult<LinkResponse>>>
{
    public LinksByUserNameParameters Parameters { get; set; } = new LinksByUserNameParameters();
}

public class GetPagedLinksByUserNameQueryHandler : IRequestHandler<GetPagedLinksByUserNameQuery, IResponseWrapper<PaginationResult<LinkResponse>>>
{
    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;

    public GetPagedLinksByUserNameQueryHandler(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }

    async Task<IResponseWrapper<PaginationResult<LinkResponse>>> IRequestHandler<GetPagedLinksByUserNameQuery, IResponseWrapper<PaginationResult<LinkResponse>>>.Handle(GetPagedLinksByUserNameQuery request, CancellationToken cancellationToken)
    {
        if (request.Parameters.UserName == null)
            return await ResponseWrapper<PaginationResult<LinkResponse>>
                .FailAsync("[ML26] User does not exist");

        var pagedResult = await _linkService.GetPagedLinksByUserNameAsync(request.Parameters);
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