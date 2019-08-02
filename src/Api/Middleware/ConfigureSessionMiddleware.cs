using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Template.Shared;
using Template.Shared.Helpers;
using Template.Shared.Session;

namespace Template.Api.Middleware
{
    // see @ https://trailheadtechnology.com/aspnetcore-multi-tenant-tips-and-tricks/
    public class ConfigureSessionMiddleware
    {
        private readonly RequestDelegate next;

        public ConfigureSessionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICurrentSession session)
        {
            if (context.User.Identities.Any(id => id.IsAuthenticated))
            {
                session.UserId = ClaimsHelper.GetClaim<int>(context.User, Constants.ClaimTypes.Id);
                session.Roles = ClaimsHelper.GetClaims<string>(context.User, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                session.UserName = ClaimsHelper.GetClaim<string>(context.User, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

                // if is a tenant, enable tenant filter
                if (session.Roles.Contains(Constants.Roles.Tenant))
                {
                    session.TenantId = session.UserId;
                }
                else
                {
                    session.DisableTenantFilter = true;
                }
            }

            await this.next.Invoke(context);
        }
    }
}
