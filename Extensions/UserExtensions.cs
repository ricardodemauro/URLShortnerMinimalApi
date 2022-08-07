using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace URLShortnerMinimalApi.Extensions
{
    public static class UserExtensions
    {
        public static string UserId([NotNull] this ClaimsPrincipal principal)
            => principal.Identity?.Name ?? string.Empty;

        public static string UserDisplay([NotNull] this ClaimsPrincipal principal)
            => principal.Claims?.FirstOrDefault(x => x.Type == "name")?.Value ?? string.Empty;
    }
}
