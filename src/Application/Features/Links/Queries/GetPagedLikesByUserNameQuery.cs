using Application.Pipelines;
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

public class GetPagedLikesByUserNameQuery : IRequest<IResponseWrapper<PaginationResult<LikePaginationResponse>>>
{
    public LikesByUserNameParameters Parameters { get; set; } = new LikesByUserNameParameters();
}

public class GetPagedLikesByUserNameQueryHandler : IRequestHandler<GetPagedLikesByUserNameQuery, IResponseWrapper<PaginationResult<LikePaginationResponse>>>
{
    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;

    public GetPagedLikesByUserNameQueryHandler(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }

    async Task<IResponseWrapper<PaginationResult<LikePaginationResponse>>> IRequestHandler<GetPagedLikesByUserNameQuery, IResponseWrapper<PaginationResult<LikePaginationResponse>>>.Handle(GetPagedLikesByUserNameQuery request, CancellationToken cancellationToken)
    {
        if (request.Parameters.UserName == null)
            return await ResponseWrapper<PaginationResult<LikePaginationResponse>>
                .FailAsync("[ML84] Like does not exist");

        var pagedResult = await _linkService.GetPagedLikesByUserNameAsync(request.Parameters);
        var mappedItems = _mapper.Map<IEnumerable<LikePaginationResponse>>(pagedResult.Items);

        return await ResponseWrapper<PaginationResult<LikePaginationResponse>>
        .SuccessAsync(new PaginationResult<LikePaginationResponse>(
            mappedItems,
            pagedResult.TotalCount,
            pagedResult.TotalPage,
            pagedResult.Page,
            pagedResult.ItemsPerPage
        ));

    }
}