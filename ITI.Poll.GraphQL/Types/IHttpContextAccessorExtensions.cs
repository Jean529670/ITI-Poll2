using System.Linq;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Http
{
    public static class IHttpContextAccessorExtensions
    {
        public static int UserId(this IHttpContextAccessor @this)
        {
            return int.Parse(@this.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}