using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using mERP.Data;
using mERP.Filters;

namespace mERP.Controllers
{
    [RaccessAuthorize]
    public class ParamsController : Controller
    {
        private readonly DbmErpContext _context;

        public ParamsController(DbmErpContext context)
        {
            _context = context;
        }

        // GET: Params
        public async Task<IActionResult> Index()
        {
            return View(await _context.Params.ToListAsync());
        }

        // GET: Params/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @param = await _context.Params
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@param == null)
            {
                return NotFound();
            }

            return View(@param);
        }

        // GET: Params/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Params/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Alphac,Descrip,Value")] Param @param)
        {
            if (ModelState.IsValid)
            {
                @param.Udate = DateTime.Now;
                @param.Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(@param);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@param);
        }

        // GET: Params/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @param = await _context.Params.FindAsync(id);
            if (@param == null)
            {
                return NotFound();
            }
            return View(@param);
        }

        // POST: Params/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Code,Alphac,Descrip,Value")] Param @param)
        {
            if (id != @param.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    @param.Udate = DateTime.Now;
                    @param.Userid = User.FindFirstValue(ClaimTypes.NameIdentifier); //usrid
                    _context.Update(@param);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParamExists(@param.Id))
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
            return View(@param);
        }

        // GET: Params/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @param = await _context.Params
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@param == null)
            {
                return NotFound();
            }

            return View(@param);
        }

        // POST: Params/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var @param = await _context.Params.FindAsync(id);
            if (@param != null)
            {
                _context.Params.Remove(@param);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParamExists(decimal id)
        {
            return _context.Params.Any(e => e.Id == id);
        }
    }
}
