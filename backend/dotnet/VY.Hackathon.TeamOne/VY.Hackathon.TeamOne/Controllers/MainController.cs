using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts;

namespace VY.Hackathon.TeamOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MainController : ControllerBase
    {
        private readonly IProviderDataService _providerDataService;

        public MainController(IProviderDataService providerDataService)
        {
            _providerDataService = providerDataService;
        }

        [HttpGet]
        [Route("simple")]
        public async Task<IActionResult> GetSimple([FromQuery] IEnumerable<string> parameters)
        {
            try
            {
                var result = await _providerDataService.GetDataFromProvider(parameters);
                if (result.Errors?.Any() ?? default)
                    return Ok(result.Result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
