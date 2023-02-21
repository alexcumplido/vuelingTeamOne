using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts;
using VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts.Models;

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

        [HttpPost]
        [Route("getData")]
        public async Task<IActionResult> GetData([FromBody] ProviderDataRequest request)
        {
            try
            {
                var result = await _providerDataService.GetDataFromProvider(request);
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
