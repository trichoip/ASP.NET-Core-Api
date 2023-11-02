using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections;
using System.Security.Claims;

namespace AspNetCore.JwtSecurity.Authorization;

public class PermissionAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private string[] _roles;
    public PermissionAuthorizeAttribute(params string[] roles) // params -> [PermissionAuthorize("Admin", "User")]
    {
        _roles = roles;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            if (context.HttpContext.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                ///
            }
            var rightClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (rightClaim != null)
            {
                string role = rightClaim.Value;
                if (_roles.Where(x => x.Equals(role, StringComparison.OrdinalIgnoreCase)).IsNullOrEmpty() == true)
                {
                    context.Result = new StatusCodeResult(403);
                }

                return;

            }
            else
            {
                context.Result = new StatusCodeResult(403);
            }
        }
        else
        {
            context.Result = new StatusCodeResult(401);
        }
    }
}

public static class CollectionExtensions
{
    public static bool IsNullOrEmpty(this IEnumerable @this)
    {
        if (@this != null)
        {
            return !@this.GetEnumerator().MoveNext();
        }

        return true;
    }
}