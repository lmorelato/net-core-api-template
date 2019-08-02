using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Template.Shared.Helpers
{
    public static class ClaimsHelper
    {
        public static T GetClaim<T>(ClaimsPrincipal claimsPrincipal, string type)
        {
            var claim = claimsPrincipal.Claims.SingleOrDefault(c => c.Type == type);
            if (claim == null)
            {
                return default;
            }

            return (T)Convert.ChangeType(claim.Value, typeof(T));
        }

        public static List<T> GetClaims<T>(ClaimsPrincipal claimsPrincipal, string type)
        {
            var claims = claimsPrincipal.Claims.Where(c => c.Type == type && !string.IsNullOrWhiteSpace(c.Value)).ToList();
            if (!claims.Any())
            {
                return default;
            }

            return (List<T>)claims.Select(c => (T)Convert.ChangeType(c.Value, typeof(T)));
        }
    }
}
