namespace Template.Shared
{
    public static class Constants
    {
        public static class Roles
        {
            public const string
                Any = "any",
                Admin = "admin",
                User = "user",
                Tenant = "tenant";
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
                Id = "id",
                Role = "role";
        }
    }
}