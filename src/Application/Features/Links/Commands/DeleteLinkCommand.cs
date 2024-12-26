using Application.Services;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Links.Commands;

public class DeleteLinkCommand : IRequest<IResponseWrapper>
{
    public int LinkId { get; set; }
}

public class DeleteLinkCommandHandler :
    IRequestHandler<DeleteLinkCommand, IResponseWrapper>
{
    private readonly ILinkService _linkService;

    public DeleteLinkCommandHandler(ILinkService linkService)
    {
        _linkService = linkService;
    }

    public async Task<IResponseWrapper> Handle(DeleteLinkCommand request, CancellationToken cancellationToken)
    {
        var linkInDb = await _linkService.GetLinkByIdAsync(request.LinkId);
        if (linkInDb is not null)
        {
            var linkId = await _linkService.DeleteLinkAsync(linkInDb);
            return await ResponseWrapper<int>.SuccessAsync(linkId, "[ML20] Link entry deleted successfully.");
        }
        else
        {
            return await ResponseWrapper.FailAsync("[ML21] Link does not exist.");
        }
    }
}
