using Application.Features.Links.Queries;
using Application.Services;
using Common.Authorization;
using Common.Responses.Pagination;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;

namespace WebApi.Controllers
{

    [Route("api/[controller]")]
    public class LanguageController : MyBaseController<LanguageController>
    {
        private readonly LanguageService _localizationService;

        public LanguageController(LanguageService localizationService)
        {
            _localizationService = localizationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMessage()
        {

            var message = _localizationService.GetKey("HelloWorld");
            return Ok(message);
        }
    }
}
