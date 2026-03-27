using bowling_tournament_MVCPRoject.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace bowling_tournament_MVCPRoject.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly PasswordHasher<object> _hasher = new();

        public bool VerifyPassword(BowlingUser user, string password)
        {
            var result = _hasher.VerifyHashedPassword(null, user.PasswordHash, password);
            return result != PasswordVerificationResult.Failed;
        }
    }
}
