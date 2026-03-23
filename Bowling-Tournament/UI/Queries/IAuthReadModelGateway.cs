using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.UI.Queries
{
    public interface IAuthReadModelGateway
    {
        Task<BowlingUser?> GetUserByUsernameAsync(string username);
    }
}
