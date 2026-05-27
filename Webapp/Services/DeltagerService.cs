using System.Net.Http.Json;
using Core.Model;

namespace WebApp.Service;

public class DeltagerApiService(HttpClient httpClient)
{
    private const string Base = "http://localhost:5243/api/Deltager";

    public async Task<List<DeltagerSvar>> GetByAktivitetIdAsync(string aktivitetId) =>
        await httpClient.GetFromJsonAsync<List<DeltagerSvar>>($"{Base}/{aktivitetId}") ?? [];

    public Task<HttpResponseMessage> UpsertAsync(DeltagerSvar deltagerSvar) =>
        httpClient.PostAsJsonAsync(Base, deltagerSvar);
}