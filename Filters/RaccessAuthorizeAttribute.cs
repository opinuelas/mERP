using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using mERP.Data;

namespace mERP.Filters;

public class RaccessAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (user.Identity?.IsAuthenticated != true)
        {
            context.Result = new ChallengeResult();
            return;
        }

        var roleIdClaim = user.FindFirst("RoleId")?.Value;
        if (!int.TryParse(roleIdClaim, out var roleId))
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
            return;
        }

        var controllerName = context.RouteData.Values["controller"] as string ?? string.Empty;

        var db = context.HttpContext.RequestServices.GetRequiredService<DbmErpContext>();

        var hasAccess = await db.Raccesses
            .AnyAsync(r => r.ParentId == roleId && r.Accctlr == controllerName);

        if (!hasAccess)
            context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
    }
}
