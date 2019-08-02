namespace Template.Shared
{
    public static class Constants
    {
        public static class Roles
        {
            public const string
                Any = "any",
                Admin = "admin",
                User = "user";
        }

        public static class Database
        {
            public const int
                DefaultVarcharMaxLength = 128,
                IdentityVarcharMaxLength = 256;
        }

        public static class ClaimTypes
        {
            public const string
                Id = "identity/claims/id",
                UserName = "identity/claims/username",
                Name = "identity/claims/name",
                Role = "identity/claims/role";
        }
    }
}