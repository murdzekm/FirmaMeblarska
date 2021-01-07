using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirmaMeblarska.Models;
using FirmaMeblarska.Data;

namespace FirmaMeblarska.Controllers
{
    public class ZespolPracownikController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZespolPracownikController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ZespolPracownik
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.ZespolPracownik.Include(z => z.Pracowniks);
            return View(await dataContext.ToListAsync());
        }

        public ActionResult CreateZespolPracownik()
        {
            ViewData["PracownikId"] = new SelectList(_context.Pracownik, "PracownikId", "Nazwisko");
            ViewData["ZespolId"] = new SelectList(_context.Zespol, "ZespolId", "Nazwa");
            var zespoll = _context.Zespol
                        .Select(s => new
                        {
                            Text = s.Nazwa,
                            Value = s.ZespolId
                        })
                        .ToList();

            ViewBag.ZespolList = new SelectList(zespoll, "Value", "Text");
            //ViewBag.Zespol = _context.Zespol;
            // ViewBag.ProjectId = new SelectList(_context.Pracownik, "PracownikId", "Nazwisko");
            return View();
        }

        // GET: ZespolPracownik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zespolPracownik = await _context.ZespolPracownik
                .Include(z => z.Pracowniks)
                .FirstOrDefaultAsync(m => m.ZespolId == id);
            if (zespolPracownik == null)
            {
                return NotFound();
            }

            return View(zespolPracownik);
        }

        // GET: ZespolPracownik/Create
        public IActionResult Create()
        {
            ViewData["PracownikId"] = new SelectList(_context.Pracownik, "PracownikId", "Email");
            return View();
        }

        // POST: ZespolPracownik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZespolId,PracownikId")] ZespolPracownik zespolPracownik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zespolPracownik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PracownikId"] = new SelectList(_context.Pracownik, "PracownikId", "Email", zespolPracownik.PracownikId);
            return View(zespolPracownik);
        }

        // GET: ZespolPracownik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zespolPracownik = await _context.ZespolPracownik.FindAsync(id);
            if (zespolPracownik == null)
            {
                return NotFound();
            }
            ViewData["PracownikId"] = new SelectList(_context.Pracownik, "PracownikId", "Email", zespolPracownik.PracownikId);
            return View(zespolPracownik);
        }

        // POST: ZespolPracownik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ZespolId,PracownikId")] ZespolPracownik zespolPracownik)
        {
            if (id != zespolPracownik.ZespolId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zespolPracownik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZespolPracownikExists(zespolPracownik.ZespolId))
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
            ViewData["PracownikId"] = new SelectList(_context.Pracownik, "PracownikId", "Email", zespolPracownik.PracownikId);
            return View(zespolPracownik);
        }

        // GET: ZespolPracownik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zespolPracownik = await _context.ZespolPracownik
                .Include(z => z.Pracowniks)
                .FirstOrDefaultAsync(m => m.ZespolId == id);
            if (zespolPracownik == null)
            {
                return NotFound();
            }

            return View(zespolPracownik);
        }

        // POST: ZespolPracownik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zespolPracownik = await _context.ZespolPracownik.FindAsync(id);
            _context.ZespolPracownik.Remove(zespolPracownik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZespolPracownikExists(int id)
        {
            return _context.ZespolPracownik.Any(e => e.ZespolId == id);
        }
    }
}
