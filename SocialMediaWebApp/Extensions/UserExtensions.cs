using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SocialMediaWebApp.Extensions
{
    public static class UserExtensions
    {
        public static string GetCurrentUserId(this ClaimsPrincipal principal)
        {
            if(principal is null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

        public static string GetCurrentUserName(this ClaimsPrincipal principal)
        {
            if (principal is null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue(ClaimTypes.Name)!;
        }
    }
}
