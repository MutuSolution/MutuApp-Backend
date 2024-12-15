using Application.Services.Identity;
using Common.Requests;
using Common.Responses;

namespace Infrastructure.Services;

internal class TokenService : ITokenService
{
    public Task<TokenResponse> GetTokenAsync(TokenRequest tokenRequest)
    {
        throw new NotImplementedException();
    }

    public Task<TokenResponse> GetRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        throw new NotImplementedException();
    }
}
