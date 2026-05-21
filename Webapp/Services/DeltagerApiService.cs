using System.Net.Http.Json;
using Core.Model;

namespace WebApp.Service;

public class DeltagerApiService(HttpClient httpClient)
{
    private const string Base = "http://localhost:5243/api/Deltager";

    public async Task<List<Deltager>> GetByAktivitetIdAsync(string aktivitetId) =>
        await httpClient.GetFromJsonAsync<List<Deltager>>($"{Base}/{aktivitetId}") ?? [];

    public Task<HttpResponseMessage> UpsertAsync(Deltager deltager) =>
        httpClient.PostAsJsonAsync(Base, deltager);
}