using Application.Services;
using AutoMapper;
using Common.Responses.Links;
using Common.Responses.Pagination;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Links.Queries;

public class GetPagedLikesByUserNameQuery : IRequest<IResponseWrapper<PaginationResult<LinkResponse>>>
{
    public LikesByUserNameParameters Parameters { get; set; } = new LikesByUserNameParameters();
}

public class GetPagedLikesByUserNameQueryHandler : IRequestHandler<GetPagedLikesByUserNameQuery, IResponseWrapper<PaginationResult<LinkResponse>>>
{
    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;

    public GetPagedLikesByUserNameQueryHandler(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }

    async Task<IResponseWrapper<PaginationResult<LinkResponse>>> IRequestHandler<GetPagedLikesByUserNameQuery, IResponseWrapper<PaginationResult<LinkResponse>>>.Handle(GetPagedLikesByUserNameQuery request, CancellationToken cancellationToken)
    {
        if (request.Parameters.UserName == null)
            return await ResponseWrapper<PaginationResult<LinkResponse>>
                .FailAsync("[ML84] Like does not exist");

        var pagedResult = await _linkService.GetPagedLikesByUserNameAsync(request.Parameters);
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