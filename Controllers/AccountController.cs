using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mERP.Data;
using mERP.Helpers;
using mERP.Models;

namespace mERP.Controllers;

public class AccountController : Controller
{
    private readonly DbmErpContext _context;
    private readonly IWebHostEnvironment _env;

    public AccountController(DbmErpContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Usrid == model.UserId && u.Usrsts == 1);

        if (user == null || !PasswordHelper.Verify(model.Password, user.Usrpwd ?? string.Empty))
        {
            ModelState.AddModelError(string.Empty, "Invalid user ID or password.");
            return View(model);
        }

        var role = await _context.Uroles
            .FirstOrDefaultAsync(r => r.Id == (decimal)(user.Usrrole ?? 0));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Usrid ?? string.Empty),
            new Claim(ClaimTypes.Name, user.Usrname ?? user.Usrid ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Usremail ?? string.Empty),
            new Claim(ClaimTypes.Role, role?.Rname ?? string.Empty),
            new Claim("RoleId", (user.Usrrole ?? 0).ToString()),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var authProps = new AuthenticationProperties
        {
            IsPersistent = model.RememberMe,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    // TEMPORARY — development only. Remove after updating the DB.
    // Usage: GET /Account/GenerateHash?pwd=yourPlaintextPassword
    /*[HttpGet]
    public IActionResult GenerateHash(string pwd)
    {
        if (!_env.IsDevelopment())
            return NotFound();

        if (string.IsNullOrEmpty(pwd))
            return BadRequest("Provide ?pwd=yourPassword");

        return Content(PasswordHelper.Hash(pwd));
    }*/
}
