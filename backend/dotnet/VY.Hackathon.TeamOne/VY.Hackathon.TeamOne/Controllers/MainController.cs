using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VY.Hackathon.TeamOne.WebApi.DataProvider.Repository;
using VY.Hackathon.TeamOne.WebApi.Mappers;
using VY.Hackaton.Entities;
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
        private readonly ResultSnapshotRepository _resultSnapshotRepository;

        public MainController(
            IProviderDataService providerDataService,
            ResultSnapshotRepository resultSnapshotRepository)
        {
            _providerDataService = providerDataService;
            _resultSnapshotRepository = resultSnapshotRepository;
        }

        [HttpPost]
        [Route("getData")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetData([FromBody] ProviderDataRequest request)
        {
            try
            {
                
                var result = await _providerDataService.GetDataFromProvider(request);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
