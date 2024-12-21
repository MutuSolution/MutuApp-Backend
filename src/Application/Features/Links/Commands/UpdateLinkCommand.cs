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
            linkInDb.Title = request.UpdateLinkRequest.Title;
            linkInDb.Url = request.UpdateLinkRequest.Url;
            linkInDb.UserName = request.UpdateLinkRequest.UserName;
            linkInDb.Description = request.UpdateLinkRequest.Description;
            linkInDb.IsPublic = request.UpdateLinkRequest.IsPublic;
            linkInDb.IsDeleted = request.UpdateLinkRequest.IsDeleted;
            linkInDb.LikeCount = request.UpdateLinkRequest.LikeCount;


            var updatedLink = await _linkService.UpdateLinkAsync(linkInDb);
            var mappedLink = _mapper.Map<LinkResponse>(updatedLink);

            return await ResponseWrapper<LinkResponse>
                .SuccessAsync(mappedLink, "[ML_03] Link updated successfully");
        }
        return await ResponseWrapper<LinkResponse>
            .FailAsync("[ML_04] Link does not exist.");
    }
}
