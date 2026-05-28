using System.Net.Http.Json;
using Core.Model;

namespace WebApp.Service;

public class TeamchatAPIService(HttpClient httpClient)
{
    private const string Base = "http://localhost:5243/api/chat";

    public async Task<List<TeamChatModel>> GetMessagesAsync() =>
        await httpClient.GetFromJsonAsync<List<TeamChatModel>>(Base) ?? [];

    public Task<HttpResponseMessage> SendMessageAsync(string user, string text) =>
        httpClient.PostAsJsonAsync(Base, new { User = user, Text = text });
    
}