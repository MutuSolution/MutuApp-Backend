using Application.Services;
using Common.Responses.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Links.Queries;

public class IsLikedQuery : IRequest<IResponseWrapper>
{
    public int LinkId { get; set; }
}

public class IsLikedQueryHandler : IRequestHandler<IsLikedQuery, IResponseWrapper>
{
    private readonly ILinkService _linkService;

    public IsLikedQueryHandler(ILinkService linkService)
    {
        _linkService = linkService;
    }

    public async Task<IResponseWrapper> Handle(IsLikedQuery request, CancellationToken cancellationToken)
    {
        return await _linkService.IsLike(request.LinkId, cancellationToken);
    }
}
