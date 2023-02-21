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
        [ProducesResponseType(typeof(ProviderDataResponse[]), 200)]
        public async Task<IActionResult> GetData([FromBody] ProviderDataRequest request)
        {
            try
            {
                var current = await _resultSnapshotRepository.GetLatestAsync();

                if (current == null)
                {
                    OperationResult<ProviderDataResponse[]> result = await _providerDataService.GetDataFromProvider(request);

                    if (result.Errors.Any())
                    {
                        return BadRequest();
                    }

                    await _resultSnapshotRepository.AddAsync(result.Result.ToEntities());

                    return Ok(result.Result);
                }

                return Ok(OperationResultHelper.GenerateOperationResult(current.ToModel()));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
