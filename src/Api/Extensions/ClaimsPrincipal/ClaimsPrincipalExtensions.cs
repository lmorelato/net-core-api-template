using System;
using System.Collections.Generic;
using System.Linq;

namespace Template.Api.Extensions.ClaimsPrincipal
{
    public static class ClaimsPrincipalExtensions
    {
        public static T GetClaim<T>(this System.Security.Claims.ClaimsPrincipal claimsPrincipal, string type)
        {
            var claim = claimsPrincipal.Claims.SingleOrDefault(c => c.Type == type);

            if (claim == null)
            {
                return default;
            }

            return (T)Convert.ChangeType(claim.Value, typeof(T));
        }

        public static List<T> GetClaims<T>(this System.Security.Claims.ClaimsPrincipal claimsPrincipal, string type)
        {
            var claims = claimsPrincipal.Claims
                .Where(c => c.Type == type && !string.IsNullOrEmpty(c.Value))
                .ToList();

            if (claims.Any())
            {
                return (List<T>)claims.Select(c => (T)Convert.ChangeType(c.Value, typeof(T)));
            }

            return new List<T>();
        }
    }
}
