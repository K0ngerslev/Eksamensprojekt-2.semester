using System.Net.Http.Json;
using Core.Model;

namespace WebApp.Service;

public class ParticipationApiService(HttpClient httpClient)
{
    private const string Base = "http://localhost:5243/api/Participation";

    public async Task<List<Participation>> GetByAktivitetIdAsync(string aktivitetId) =>
        await httpClient.GetFromJsonAsync<List<Participation>>($"{Base}/{aktivitetId}") ?? [];

    public Task<HttpResponseMessage> UpsertAsync(Participation participation) =>
        httpClient.PostAsJsonAsync(Base, participation);
}