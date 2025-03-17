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

namespace Application.Features.Links.Queries.Home;

public class GetPublicLinkLikeWithUsernameQuery : IRequest<IResponseWrapper>
{
    public string UserName { get; set; }
}

public class GetPublicLinkLikeWithUsernameQueryHandler : IRequestHandler<GetPublicLinkLikeWithUsernameQuery, IResponseWrapper>
{
    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;
    public GetPublicLinkLikeWithUsernameQueryHandler(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }
  

    public async Task<IResponseWrapper> Handle(GetPublicLinkLikeWithUsernameQuery request, CancellationToken cancellationToken)
    {
        return await _linkService.PublicLinkLikeWithUsername(request.UserName);

    }
}
