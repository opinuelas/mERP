using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mERP.Data;
using mERP.Filters;
using mERP.Helpers;

namespace mERP.Controllers
{
    [RaccessAuthorize]
    public class UsersController : Controller
    {
        private readonly DbmErpContext _context;

        public UsersController(DbmErpContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            ViewBag.RoleNames = await _context.Uroles
                .ToDictionaryAsync(r => (int)r.Id, r => r.Rname);
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/RstPwd/5
        public async Task<IActionResult> RstPwd(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.Usrpwd = string.Empty; // Clear the password field for security reasons
            }

            return View(user);
        }

        // POST: Users/RstPwd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RstPwd(int id, [Bind("Id,Usrpwd")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(user.Usrid));
            ModelState.Remove(nameof(user.Usrname));
            ModelState.Remove(nameof(user.Usrrole));

            if (string.IsNullOrWhiteSpace(user.Usrpwd))
            {
                ModelState.AddModelError(nameof(user.Usrpwd), "Password is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Users.FindAsync(id);
                    if (existing == null) return NotFound();

                    existing.Usrpwd = PasswordHelper.Hash(user.Usrpwd!);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessMessage"] = "The password has been reset.";
                return RedirectToAction(nameof(RstPwd), new { id });
            }

            // Reload from DB to restore display-only fields (Usrid, Usrname)
            var displayUser = await _context.Users.FindAsync(id);
            if (displayUser == null) return NotFound();
            displayUser.Usrpwd = user.Usrpwd;
            return View(displayUser);
        }




        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Usrrole = new SelectList(await _context.Uroles.OrderBy(r => r.Rname).ToListAsync(), "Id", "Rname");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Usrid,Usrname,Usremail,Usrcell,Usrrole,Usrpar,Usriniop")] User user)
        {
            if (await _context.Users.AnyAsync(u => u.Usrid == user.Usrid))
                ModelState.AddModelError(nameof(user.Usrid), "This user id already exists.");

            if (ModelState.IsValid)
            {
                var defaultPwd = await _context.Params
                    .Where(p => p.Code == "default_pwd")
                    .Select(p => p.Value)
                    .FirstOrDefaultAsync() ?? string.Empty;
                user.Usrpwd = PasswordHelper.Hash(defaultPwd);
                user.Usrsts=1; // Set the user status to active (1)

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Usrrole = new SelectList(await _context.Uroles.OrderBy(r => r.Rname).ToListAsync(), "Id", "Rname", user.Usrrole);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Usrrole = new SelectList(await _context.Uroles.OrderBy(r => r.Rname).ToListAsync(), "Id", "Rname", user.Usrrole);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Usrid,Usrname,Usremail,Usrcell,Usrsts,Usrrole,Usrpar,Usriniop")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (await _context.Users.AnyAsync(u => u.Usrid == user.Usrid && u.Id != id))
                ModelState.AddModelError(nameof(user.Usrid), "This user id already exists.");

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Users.FindAsync(id);
                    if (existing == null) return NotFound();

                    existing.Usrid    = user.Usrid;
                    existing.Usrname  = user.Usrname;
                    existing.Usremail = user.Usremail;
                    existing.Usrcell  = user.Usrcell;
                    existing.Usrsts   = user.Usrsts;
                    existing.Usrrole  = user.Usrrole;
                    existing.Usrpar   = user.Usrpar;
                    existing.Usriniop = user.Usriniop;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Usrrole = new SelectList(await _context.Uroles.OrderBy(r => r.Rname).ToListAsync(), "Id", "Rname", user.Usrrole);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
