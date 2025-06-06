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

public class SendEmailConfirmQuery : IRequest<ResponseWrapper>
{
    public SendEmailConfirmRequest Request { get; set; }
}

public class SendEmailQueryHandler : IRequestHandler<SendEmailConfirmQuery, ResponseWrapper>
{
    private readonly IEmailService _emailService;
    public SendEmailQueryHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }
    public async Task<ResponseWrapper> Handle(SendEmailConfirmQuery request, CancellationToken cancellationToken)
    {
        return await _emailService.SendEmailConfirmAsync(request.Request);
    }
}
