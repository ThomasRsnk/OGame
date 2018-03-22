using System.Linq;
using System.Security.Principal;

namespace Djm.OGame.Web.Api.Extensions
{
    public static class PrincipalExtensions
    {
        public static bool IsInAllRoles(this IPrincipal principal, params string[] roles)
        {
            return roles.All(principal.IsInRole);
        }

        public static bool IsInAnyRoles(this IPrincipal principal, params string[] roles)
        {
            return roles.Any(principal.IsInRole);
        }
    }
}