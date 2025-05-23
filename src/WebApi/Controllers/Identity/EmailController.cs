﻿namespace WebApi.Controllers.Identity;

using Application.Features.Identity.Token.Queries;
using Application.Features.Links.Commands;
using Common.Authorization;
using Common.Requests.Identity;
using Common.Requests.Links;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;

[Route("api/[controller]")]
public class EmailController : MyBaseController<EmailController>
{
    [HttpPost("send-confirm-email")]
    [AllowAnonymous]
    public async Task<IActionResult> SendConfirmEmail([FromBody] SendEmailConfirmRequest request)
    {
        var response = await MediatorSender.Send(new SendEmailConfirmQuery { Request = request });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPost("confirm-email")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmRequest request)
    {

        var response = await MediatorSender.Send(new GetEmailConfirmQuery { EmailConfirmRequest = request });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
