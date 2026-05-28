using Core.Model;

namespace ServerAPI.Repositories;

public interface ITeamchatRepository
{
    Task<List<TeamChatModel>> GetMessagesAsync();
    Task AddMessageAsync(string user, string text);
}
