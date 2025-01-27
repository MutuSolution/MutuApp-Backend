using Application.Pipelines;
using Application.Services;
using AutoMapper;
using Common.Requests.Links;
using Common.Responses.Links;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Links.Commands;

public class UpdateLinkCommand : IRequest<IResponseWrapper>, IValidateMe
{
    public UpdateLinkRequest UpdateLinkRequest { get; set; }
}

public class UpdateLinkCommandHandler : IRequestHandler<UpdateLinkCommand, IResponseWrapper>
{
    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;

    public UpdateLinkCommandHandler(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }

    public async Task<IResponseWrapper> Handle(UpdateLinkCommand request,
        CancellationToken cancellationToken)
    {
        var linkInDb = await _linkService
            .GetLinkByIdAsync(request.UpdateLinkRequest.Id);
        if (linkInDb is not null)
        {
            var updatedLink = new LinkResponse
            {
                Id = linkInDb.Id,
                Title = request.UpdateLinkRequest.Title,
                Url = request.UpdateLinkRequest.Url,
                UserName = request.UpdateLinkRequest.UserName,
                Description = request.UpdateLinkRequest.Description,
                IsPublic = request.UpdateLinkRequest.IsPublic,
                IsDeleted = request.UpdateLinkRequest.IsDeleted,
                LikeCount = request.UpdateLinkRequest.LikeCount
            };

            var result = await _linkService.UpdateLinkAsync(updatedLink);
            var mappedLink = _mapper.Map<LinkResponse>(result);

            return await ResponseWrapper<LinkResponse>
                .SuccessAsync(mappedLink, "[ML22] Link updated successfully");
        }
        return await ResponseWrapper<LinkResponse>
            .FailAsync("[ML23] Link does not exist.");
    }
}
