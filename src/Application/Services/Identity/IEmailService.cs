using Common.Requests.Identity;
using Common.Responses.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Identity;

public interface IEmailService
{
    Task<ResponseWrapper> SendEmailConfirmAsync(SendEmailConfirmRequest request);
    Task<IResponseWrapper> GetEmailConfirmAsync(EmailConfirmRequest emailConfirmRequest);
}
