using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DevryDeveloperClub.Infrastructure.Extensions
{
    public static class ClaimExtensions
    {
        public static bool ClaimExists(this IEnumerable<Claim> claims, string type, out string value)
        {
            value = string.Empty;
            
            if (claims == null)
                return false;
            
            if (claims.All(x => x.Type != type))
                return false;

            value = claims.First(x => x.Type == type).Value;

            return true;
        }
    }
}