using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.Domain.Services
{
    public interface IAuthService
    {
        bool VerifyPassword(BowlingUser user, string password);
    }
}
