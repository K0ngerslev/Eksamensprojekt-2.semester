using System.Net.Http.Json;
using Core.Model;

namespace WebApp.Service;

public class DeltagerApiService(HttpClient httpClient)
{
    private const string Base = "api/Deltager";

    public async Task<List<DeltagerSvar>> GetByAktivitetIdAsync(string aktivitetId)
    {
// Trin 1: Send request og få HTTP-svaret tilbage
        var response = await httpClient.GetAsync($"{Base}/{aktivitetId}");

// Trin 2: Tjek om det gik godt FØR du forsøger at læse data
        if (!response.IsSuccessStatusCode)
            return [];

// Trin 3: Deserialiser kun hvis svaret var OK
        return await response.Content.ReadFromJsonAsync<List<DeltagerSvar>>() ?? [];
    }

    public Task<HttpResponseMessage> UpsertAsync(DeltagerSvar deltagerSvar) =>
        httpClient.PostAsJsonAsync(Base, deltagerSvar);
}