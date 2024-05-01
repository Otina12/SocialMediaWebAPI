using SocialMedia.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Application.Helpers.Extensions
{
    public static class UserExtensions
    {
        public static string GetCurrentUserId(this ClaimsPrincipal principal)
        {
            if (principal is null)
                throw new UserNotFoundException("Login required");

            return principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

        public static string GetCurrentUserName(this ClaimsPrincipal principal)
        {
            if (principal is null)
                throw new UserNotFoundException("Login required");

            return principal.FindFirstValue(ClaimTypes.Name)!;
        }
    }
}
