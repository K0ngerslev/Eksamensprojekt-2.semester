using System.Net.Http.Json;
using Core.Model;

namespace WebApp.Service;

public class AktivitetApiService
{
    private readonly HttpClient _httpClient;

    public AktivitetApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Aktivitet>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Aktivitet>>("http://localhost:5243/api/Aktivitet") ?? [];
    }

    public Task<HttpResponseMessage> AddAsync(Aktivitet aktivitet)
    {
        return _httpClient.PostAsJsonAsync("http://localhost:5243/api/Aktivitet", aktivitet);
    }
}