using bowling_tournament_MVCPRoject.Domain.Entities;
using bowling_tournament_MVCPRoject.UI.Queries;
using Microsoft.EntityFrameworkCore;

namespace bowling_tournament_MVCPRoject.Persistence.Queries
{
    public class AuthReadModelGateway : IAuthReadModelGateway
    {
        private readonly BowlingDbContextV2 _context;
        public AuthReadModelGateway(BowlingDbContextV2 context)
        {
            _context = context;
        }
        public async Task<BowlingUser?> GetUserByUsernameAsync(string username)
        {
            return await _context.BowlingUser.FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
