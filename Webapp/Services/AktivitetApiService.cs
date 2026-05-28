using System.Net.Http.Json;
using Core.Model;

namespace WebApp.Service;

public class AktivitetApiService
{
    private readonly HttpClient httpClient;

    public AktivitetApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<AktivitetModel>> GetAllAsync()
    {
        return await httpClient.GetFromJsonAsync<List<AktivitetModel>>("http://localhost:5243/api/Aktivitet") ?? [];
    }

    public Task<HttpResponseMessage> AddAsync(AktivitetRequestModel aktivitet)
    {
        return httpClient.PostAsJsonAsync("http://localhost:5243/api/Aktivitet", aktivitet);
    }

    public Task<HttpResponseMessage> UpdateAsync(string id, AktivitetRequestModel aktivitet)
    {
        return httpClient.PutAsJsonAsync($"http://localhost:5243/api/Aktivitet/{id}", aktivitet);
    }

    public Task<HttpResponseMessage> DeleteAsync(string id)
    {
        return httpClient.DeleteAsync($"http://localhost:5243/api/Aktivitet/{id}");
    }
}
