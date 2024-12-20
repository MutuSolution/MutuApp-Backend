using Application.Pipelines;
using Application.Services;
using AutoMapper;
using Common.Requests.Links;
using Common.Responses.Links;
using Common.Responses.Wrappers;
using Domain;
using MediatR;

namespace Application.Features.Links.Commands;

public class CreateLinkCommand : IRequest<IResponseWrapper>, IValidateMe
{
    public CreateLinkRequest CreateLinkRequest { get; set; }
}

public class CreateLinkCommandHandler :
    IRequestHandler<CreateLinkCommand, IResponseWrapper>
{
    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;

    public CreateLinkCommandHandler(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }

    public async Task<IResponseWrapper> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        var mappedLink = _mapper.Map<Link>(request.CreateLinkRequest);
        var newLink = await _linkService.CreateLinkAsync(mappedLink);
        if (newLink.Id > 0)
        {
            var mappedNewLink = _mapper.Map<LinkResponse>(newLink);
            return await ResponseWrapper<LinkResponse>
                .SuccessAsync(mappedNewLink, "Link created successfully.");
        }
        return await ResponseWrapper.FailAsync("Failed to create link entry.");
    }
}