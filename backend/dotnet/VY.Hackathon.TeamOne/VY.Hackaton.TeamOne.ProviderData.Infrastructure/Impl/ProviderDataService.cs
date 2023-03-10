using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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

    public async Task<string> GetDataFromProvider(ProviderDataRequest request)
    {
        try
        {
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("getData", httpContent);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);
            return json;
        }
        catch (Exception ex)
        {
            return ex.StackTrace;
        }
    }
}