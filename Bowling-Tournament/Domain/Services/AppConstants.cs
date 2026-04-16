namespace bowling_tournament_MVCPRoject.Domain.Services
{
    public static class AppConstants
    {
        public const decimal TEAM_REGISTRATION_FEE = 200.00m;

        public static class DivisionIds
        {
            public const int MEN = 1;
            public const int WOMEN = 2;
            public const int MIXED = 3;
            public const int YOUTH = 4;
            public const int SENIOR = 5;
        }

        public static class AuthorizationClaims
        {
            public const string IS_ADMIN = "IsAdmin";
            public const string ADMIN_VALUE = "true";
        }
    }
}
