using System.Net.Http.Json;
using Core.Model;

namespace WebApp.Service;

public class TeamchatAPIService(HttpClient httpClient)
{
    private const string Base = "api/chat";

    public async Task<List<TeamChatModel>> GetMessagesAsync()
    {
        var response = await httpClient.GetAsync(Base);
        if (!response.IsSuccessStatusCode)
            return [];
        return await response.Content.ReadFromJsonAsync<List<TeamChatModel>>() ?? [];
    }

    public Task<HttpResponseMessage> SendMessageAsync(string user, string text) =>
        httpClient.PostAsJsonAsync(Base, new { User = user, Text = text });
    
}