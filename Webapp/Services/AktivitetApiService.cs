using System.Net.Http.Json;
using Core.Model;

namespace WebApp.Service;

public class AktivitetApiService
{
    private readonly HttpClient httpClient;
    private const string Base = "http://localhost:5243/api/Aktivitet";

    public AktivitetApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<AktivitetModel>> GetAllAsync()
    {
        var response = await httpClient.GetAsync(Base);
        if (!response.IsSuccessStatusCode)
            return [];
        return await response.Content.ReadFromJsonAsync<List<AktivitetModel>>() ?? [];
    }

    public Task<HttpResponseMessage> AddAsync(AktivitetRequestModel aktivitet)
        => httpClient.PostAsJsonAsync(Base, aktivitet);

    public Task<HttpResponseMessage> UpdateAsync(string id, AktivitetRequestModel aktivitet)
        => httpClient.PutAsJsonAsync($"{Base}/{id}", aktivitet);

    public Task<HttpResponseMessage> DeleteAsync(string id)
        => httpClient.DeleteAsync($"{Base}/{id}");
}