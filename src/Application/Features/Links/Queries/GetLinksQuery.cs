using Application.Services;
using AutoMapper;
using Common.Responses.Links;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Links.Queries;

public class GetLinksQuery : IRequest<IResponseWrapper>
{

}

public class GetLinksQueryHandler : IRequestHandler<GetLinksQuery, IResponseWrapper>
{
    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;

    public GetLinksQueryHandler(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }

    public async Task<IResponseWrapper> Handle(GetLinksQuery request,
        CancellationToken cancellationToken)
    {
        var linkList = await _linkService.GetLinkListAsync();
        if (linkList.Count > 0)
        {
            var mappedLinkList = _mapper.Map<List<LinkResponse>>(linkList);
            return await ResponseWrapper<List<LinkResponse>>
                .SuccessAsync(mappedLinkList);
        }
        return await ResponseWrapper.FailAsync("No links were found.");
    }
}
