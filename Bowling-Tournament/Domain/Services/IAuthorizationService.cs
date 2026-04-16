using System.Security.Claims;

namespace bowling_tournament_MVCPRoject.Domain.Services
{
    public interface IUserAuthorizationService
    {
        bool IsAdmin(ClaimsPrincipal user);
    }

    public class UserAuthorizationService : IUserAuthorizationService
    {
        public bool IsAdmin(ClaimsPrincipal user)
        {
            return user?.HasClaim(AppConstants.AuthorizationClaims.IS_ADMIN, AppConstants.AuthorizationClaims.ADMIN_VALUE) ?? false;
        }
    }
}
