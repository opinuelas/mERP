using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using mERP.Data;

namespace mERP.Filters;

public class RaccessAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    // accval positions: 0=Consult, 1=Create, 2=Edit, 3=Delete/ChgPwd
    private static readonly Dictionary<string, int> ActionPositionMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Index",   0 },
        { "Details", 0 },
        { "Create",  1 },
        { "Edit",    2 },
        { "Delete",  3 },
        { "RstPwd",  3 },
    };

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
        var actionName     = context.RouteData.Values["action"]     as string ?? string.Empty;

        var db = context.HttpContext.RequestServices.GetRequiredService<DbmErpContext>();

        var accval = await db.Raccesses
            .Where(r => r.ParentId == roleId && r.Accctlr == controllerName)
            .Select(r => r.Accval)
            .FirstOrDefaultAsync();

        if (accval == null)
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
            return;
        }

        if (!ActionPositionMap.TryGetValue(actionName, out var position))
            return; // unknown action — controller-level access is enough

        if (accval.Length <= position || accval[position] != 'Y')
            context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
    }
}
