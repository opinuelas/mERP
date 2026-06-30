using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mERP.Data;
using mERP.Filters;

namespace mERP.Controllers
{
    [RaccessAuthorize]
    public class MyopsController : Controller
    {
        private readonly DbmErpContext _context;

        public MyopsController(DbmErpContext context)
        {
            _context = context;
        }

        // GET: Myops
        public async Task<IActionResult> Index()
        {
            var roleIdClaim = User.FindFirstValue("RoleId");
            if (!int.TryParse(roleIdClaim, out var roleId))
                return Forbid();

            var menus = await _context.Raccesses
                .Where(r => r.ParentId == roleId)
                .OrderBy(r => r.Accord)
                .ToListAsync();

            return View(menus);
        }

    }
}
