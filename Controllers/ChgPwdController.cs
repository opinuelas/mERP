using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mERP.Data;
using mERP.Helpers;
using mERP.Models;
using mERP.Filters;

namespace mERP.Controllers;

[RaccessAuthorize]
public class ChgPwdController : Controller
{
    private readonly DbmErpContext _context;

    public ChgPwdController(DbmErpContext context)
    {
        _context = context;
    }

    // GET: ChgPwd
    public async Task<IActionResult> Index()
    {
        var usrid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Usrid == usrid);
        if (user == null) return NotFound();

        ViewBag.Usrid = user.Usrid;
        return View(new ChgPwdViewModel());
    }

    // POST: ChgPwd
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ChgPwdViewModel model)
    {
        var usrid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Usrid == usrid);
        if (user == null) return NotFound();

        ViewBag.Usrid = user.Usrid;

        if (!ModelState.IsValid)
            return View(model);

        if (!PasswordHelper.Verify(model.CurrentPassword, user.Usrpwd ?? string.Empty))
        {
            ModelState.AddModelError(nameof(model.CurrentPassword), "Incorrect Current Password");
            return View(model);
        }

        if (model.NewPassword != model.ConfirmPassword)
        {
            ModelState.AddModelError(nameof(model.ConfirmPassword), "New and Confirm Password are not the same");
            return View(model);
        }

        user.Usrpwd = PasswordHelper.Hash(model.NewPassword);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Password has been changed";
        return RedirectToAction(nameof(Index));
    }
}
