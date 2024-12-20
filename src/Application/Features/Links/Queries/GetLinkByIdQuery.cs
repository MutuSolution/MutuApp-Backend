using Application.Services;
using AutoMapper;
using Common.Responses.Links;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Links.Queries;

public class GetLinkByIdQuery : IRequest<IResponseWrapper>
{
    public int LinkId { get; set; }
}

public class GetLinkByIdQueryHandler :
    IRequestHandler<GetLinkByIdQuery, IResponseWrapper>
{
    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;

    public GetLinkByIdQueryHandler(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }

    public async Task<IResponseWrapper> Handle(GetLinkByIdQuery request,
        CancellationToken cancellationToken)
    {
        var linkInDb = await _linkService.GetLinkByIdAsync(request.LinkId);
        if (linkInDb is null)
            return await ResponseWrapper.FailAsync("[ML_05] Link does not exist.");

        var mappedLink = _mapper.Map<LinkResponse>(linkInDb);
        return await ResponseWrapper<LinkResponse>.SuccessAsync(mappedLink);
    }
}
