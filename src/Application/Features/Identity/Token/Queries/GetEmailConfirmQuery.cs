﻿using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Token.Queries;

public class GetEmailConfirmQuery : IRequest<IResponseWrapper>
{
    public EmailConfirmRequest EmailConfirmRequest { get; set; }
}

public class GetEmailConfirmQueryHandler : IRequestHandler<GetEmailConfirmQuery, IResponseWrapper>
{
    private readonly IEmailService _emailService;

    public GetEmailConfirmQueryHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<IResponseWrapper> Handle(GetEmailConfirmQuery request, CancellationToken cancellationToken)
    {
        return await _emailService.GetEmailConfirmAsync(request.EmailConfirmRequest);
    }
}
