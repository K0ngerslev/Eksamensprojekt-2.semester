using System.Net.Http.Json;

namespace WebApp.Service;

public class TeamchatAPIService(HttpClient httpClient)
{
    private const string Base = "http://localhost:5243/api/chat";

    public async Task<List<ChatMessage>> GetMessagesAsync() =>
        await httpClient.GetFromJsonAsync<List<ChatMessage>>(Base) ?? [];

    public Task<HttpResponseMessage> SendMessageAsync(string user, string text) =>
        httpClient.PostAsJsonAsync(Base, new { User = user, Text = text });

    public class ChatMessage
    {
        public string User { get; set; } = "";
        public string Text { get; set; } = "";
        public DateTime Time { get; set; }
    }
}