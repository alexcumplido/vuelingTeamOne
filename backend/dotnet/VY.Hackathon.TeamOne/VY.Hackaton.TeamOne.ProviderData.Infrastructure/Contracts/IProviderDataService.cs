using VY.Hackaton.Entities;
using VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts.Models;

namespace VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts;

public interface IProviderDataService
{
    Task<OperationResult<ProviderDataResponse>> GetDataFromProvider(ProviderDataRequest request);
}