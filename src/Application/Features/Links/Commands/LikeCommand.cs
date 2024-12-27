using Application.Services;
using Common.Requests.Links;
using Common.Responses.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Links.Commands;

public class LikeCommand : IRequest<IResponseWrapper>
{
    public LikeLinkRequest LikeRequest { get; set; }
}

public class LikeCommandHandler : IRequestHandler<LikeCommand, IResponseWrapper>
{
    private readonly ILinkService _linkService;

    public LikeCommandHandler(ILinkService linkService)
    {
        _linkService = linkService;
    }

    public async Task<IResponseWrapper> Handle(LikeCommand request, CancellationToken cancellationToken)
    {
        return await _linkService.LikeLinkAsync(request.LikeRequest, cancellationToken);
    }
}
