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

public class SoftDeleteLinkCommand : IRequest<IResponseWrapper>
{
    public SoftDeleteLinkRequest SoftDeleteLinkRequest { get; set; }
}

public class SoftDeleteLinkCommandHandler : IRequestHandler<SoftDeleteLinkCommand, IResponseWrapper>
{
    private readonly ILinkService _linkService;

    public SoftDeleteLinkCommandHandler(ILinkService linkService)
    {
        _linkService = linkService;
    }

    public async Task<IResponseWrapper> Handle(SoftDeleteLinkCommand request, CancellationToken cancellationToken)
    {
        return await _linkService.SoftDeleteLink(request.SoftDeleteLinkRequest);
    }
}
