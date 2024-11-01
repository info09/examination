using IdentityModel;
using System.Security.Claims;

namespace PortalApp.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity!)
                .Claims
                .SingleOrDefault(x => x.Type == JwtClaimTypes.Subject);
            return claim.Value;
        }

        public static string GetFullName(this ClaimsPrincipal claimsPrincipal)
        {
            var firstName = claimsPrincipal.FindFirst(ClaimTypes.GivenName)?.Value;
            var lastName = claimsPrincipal.FindFirst(ClaimTypes.Surname)?.Value;

            return firstName + " " + lastName;
        }
    }
}
