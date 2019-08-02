using Microsoft.AspNetCore.Hosting;

namespace Template.Api.Extensions.HostingEnvironment
{
    public static partial class HostingEnvironmentExtensions
    {
        public static bool IsTrusted(this IHostingEnvironment env)
        {
            return env.IsDevelopment() || env.IsStaging();
        }
    }
}
