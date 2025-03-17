using Application.Services.Identity;
using AutoMapper;
using Azure;
using Common.Requests.Identity;
using Common.Responses;
using Common.Responses.Identity;
using Common.Responses.Wrappers;
using Infrastructure.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Identity;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public EmailService(IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _configuration = configuration;
        _userManager = userManager;
        _mapper = mapper;
    }

    // The code appears to be mostly correct, but there are a few improvements and error handling that can be added:

    public async Task<ResponseWrapper> SendEmailConfirmAsync(SendEmailConfirmRequest request)
    {
        if (request.Email is null)
            return (ResponseWrapper)await ResponseWrapper.FailAsync("[ML91] Email required.");

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return (ResponseWrapper)await ResponseWrapper.FailAsync("[ML92] User does not exist.");

        if (user.EmailConfirmed)
            return (ResponseWrapper)await ResponseWrapper.FailAsync("[ML93] User already confirmed.");

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        // 📌 **HTML Şablonu (Butonsuz)**
        string htmlBody = $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Email Confirmation</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                margin: 0;
                padding: 0;
            }}
            .container {{
                max-width: 600px;
                margin: 20px auto;
                background-color: #ffffff;
                padding: 20px;
                border-radius: 10px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                border-top: 5px solid #FF6B00;
                text-align: center;
            }}
            .header {{
                font-size: 22px;
                font-weight: bold;
                color: #007ACC;
                padding: 10px 0;
            }}
            .code {{
                font-size: 26px;
                font-weight: bold;
                color: #FF6B00;
                background-color: #f4f4f4;
                display: inline-block;
                padding: 10px 20px;
                border-radius: 5px;
                margin: 20px 0;
            }}
            .footer {{
                font-size: 12px;
                color: #666;
                padding: 10px;
                margin-top: 20px;
                border-top: 1px solid #ddd;
            }}
        </style>
    </head>
    <body>

    <div class='container'>
        <div class='header'>Mutuapp Email Confirmation</div>
        <p>Your confirmation code:</p>
        <div class='code'>{code}</div>
        <p>Please enter this code in the app to verify your email.</p>
    </div>

    <div class='footer'>
        <p>If you didn't request this email, please ignore it.</p>
        <p>&copy; 2024 Mutuapp. All rights reserved.</p>
    </div>

    </body>
    </html>";

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("confirm@mutuapp.com"));
        email.To.Add(MailboxAddress.Parse(request.Email));
        email.Subject = "Mutuapp Email Confirmation";
        email.Body = new TextPart(TextFormat.Html) { Text = htmlBody };

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_configuration["EmailHost"], 465, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(_configuration["EmailUserName"], _configuration["EmailPassword"]);
            await client.SendAsync(email);
        }
        catch (Exception ex)
        {
            return (ResponseWrapper)await ResponseWrapper.FailAsync($"[ML90] Email sending failed: {ex.Message}");
        }
        finally
        {
            await client.DisconnectAsync(true);
        }

        return await ResponseWrapper<string>.SuccessAsync("[ML94] Verification code has been sent.");
    }

    public async Task<IResponseWrapper> GetEmailConfirmAsync(EmailConfirmRequest emailConfirmRequest)
    {
        if (emailConfirmRequest.Code is null)
            return await ResponseWrapper<TokenResponse>.FailAsync("[ML85] Code required.");
        if (emailConfirmRequest.Email is null)
            return await ResponseWrapper<TokenResponse>.FailAsync("[ML86] Email required.");

        var user = await _userManager.FindByEmailAsync(emailConfirmRequest.Email);
        if (user is null)
            return await ResponseWrapper<TokenResponse>.FailAsync("[ML87] User not found.");

        var isVerified = await _userManager.ConfirmEmailAsync(user, emailConfirmRequest.Code);
        if (!isVerified.Succeeded)
            return await ResponseWrapper<TokenResponse>.FailAsync("[ML88] Email not confirmed.");

        return await ResponseWrapper<TokenResponse>.SuccessAsync("[ML89] Email confirmed.");
    }
}
