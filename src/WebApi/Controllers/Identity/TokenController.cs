using Application.Features.Identity.Queries;
using Common.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    public class TokenController : MyBaseController<TokenController>
    {
        [HttpPost("get-token")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequest tokenRequest)
        {
            var response = await MediatorSender.Send(new GetTokenQuery { TokenRequest = tokenRequest });
            if (!response.IsSuccessful) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("refresh-token")]
        public async Task<IActionResult> GetRefreshTokenAsync(
            [FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var response = await MediatorSender
                .Send(new GetRefreshTokenQuery { RefreshTokenRequest = refreshTokenRequest});
            if (!response.IsSuccessful) return BadRequest(response);
            return Ok(response);
        }
    }
}
