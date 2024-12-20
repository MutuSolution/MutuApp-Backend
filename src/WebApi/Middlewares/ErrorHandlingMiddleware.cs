using Application.Exceptions;
using Common.Responses;
using System.Net;
using System.Text.Json;

namespace WebApi.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            var response = httpContext.Response;
            response.ContentType = "application/json";
            Error error = new();
            switch (ex)
            {
                case CustomValidationException customValidationException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    error.FriendlyErrorMessage = customValidationException.FriendlyErrorMessage;
                    error.ErrorMessages = customValidationException.ErrorMessages;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    error.FriendlyErrorMessage = ex.Message;
                    break;
            }

            var result = JsonSerializer.Serialize(error);
            await response.WriteAsync(result);
        }
    }
}
