using System.Net.Http.Json;
using VY.Hackaton.Entities;
using VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts;
using VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts.Models;

namespace VY.Hackaton.TeamOne.ProviderData.Infrastructure.Impl;

public class ProviderDataService : IProviderDataService
{
    private readonly HttpClient _httpClient;


    public ProviderDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OperationResult<ProviderDataResponse>> GetDataFromADayAndAnArea(string handlingArea, DateTime date)
    {
        try
        {
            var response = await _httpClient.GetAsync("/simple");
            response.EnsureSuccessStatusCode();
            var responseModel = await response.Content.ReadFromJsonAsync<ProviderDataResponse>() ?? default;
            if (responseModel != null)
                return OperationResultHelper.GenerateOperationResult<ProviderDataResponse>(responseModel);
            throw new ArgumentNullException(paramName: nameof(ProviderDataResponse), message:"Data received by provider is null");
        }
        catch (Exception ex)
        {
            return OperationResultHelper.GenerateOperationResultWithError<ProviderDataResponse>(ex);
        }

    }
}