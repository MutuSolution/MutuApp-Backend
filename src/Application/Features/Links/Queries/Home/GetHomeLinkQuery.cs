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

public class GetHomeLinkQuery : IRequest<IResponseWrapper>
{

}

public class GetHomeLinkQueryHandler : IRequestHandler<GetHomeLinkQuery, IResponseWrapper>
{
    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;

    public GetHomeLinkQueryHandler(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }
    public async Task<IResponseWrapper> Handle(GetHomeLinkQuery request, CancellationToken cancellationToken)
    {
        var linkList = await _linkService.GetHomeLinkListAsync();
        if (linkList.Count > 0)
        {
            var mappedLinkList = _mapper.Map<List<LinkResponse>>(linkList);
            return await ResponseWrapper<List<LinkResponse>>
                .SuccessAsync(mappedLinkList);
        }
        return await ResponseWrapper.FailAsync("[ML96] No links were found.");
    }
}
