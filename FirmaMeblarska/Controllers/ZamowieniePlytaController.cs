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
    public class ZamowieniePlytaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZamowieniePlytaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ZamowieniePlyta
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.ZamowieniePlyta.Include(z => z.Plyta).Include(z => z.Zamowienie);
            return View(await dataContext.ToListAsync());
        }

       

        // GET: ZamowieniePlyta/Create
        public IActionResult Create()
        {
            //loadDDL();
            ViewData["PlytaId"] = new SelectList(_context.Plyta, "PlytaId", "Kod");
            ViewData["ZamowienieId"] = new SelectList(_context.Zamowienie, "ZamowienieId", "ZamowienieId");
            return View();
        }

        // POST: ZamowieniePlyta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZamowienieId,PlytaId")] ZamowieniePlyta zamowieniePlyta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zamowieniePlyta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlytaId"] = new SelectList(_context.Plyta, "PlytaId", "Kod", zamowieniePlyta.PlytaId);
            ViewData["ZamowienieId"] = new SelectList(_context.Zamowienie, "ZamowienieId", "ZamowienieId", zamowieniePlyta.ZamowienieId);
            return View(zamowieniePlyta);
        }

        // GET: ZamowieniePlyta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowieniePlyta = await _context.ZamowieniePlyta.FindAsync(id);
            if (zamowieniePlyta == null)
            {
                return NotFound();
            }
            ViewData["PlytaId"] = new SelectList(_context.Plyta, "PlytaId", "Kod", zamowieniePlyta.PlytaId);
            ViewData["ZamowienieId"] = new SelectList(_context.Zamowienie, "ZamowienieId", "ZamowienieId", zamowieniePlyta.ZamowienieId);
            return View(zamowieniePlyta);
        }

        // POST: ZamowieniePlyta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ZamowienieId,PlytaId")] ZamowieniePlyta zamowieniePlyta)
        {
            if (id != zamowieniePlyta.ZamowienieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zamowieniePlyta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZamowieniePlytaExists(zamowieniePlyta.ZamowienieId))
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
            ViewData["PlytaId"] = new SelectList(_context.Plyta, "PlytaId", "Kod", zamowieniePlyta.PlytaId);
            ViewData["ZamowienieId"] = new SelectList(_context.Zamowienie, "ZamowienieId", "ZamowienieId", zamowieniePlyta.ZamowienieId);
            return View(zamowieniePlyta);
        }

        // GET: ZamowieniePlyta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowieniePlyta = await _context.ZamowieniePlyta
                .Include(z => z.Plyta)
                .Include(z => z.Zamowienie)
                .FirstOrDefaultAsync(m => m.ZamowienieId == id);
            if (zamowieniePlyta == null)
            {
                return NotFound();
            }

            return View(zamowieniePlyta);
        }

        // POST: ZamowieniePlyta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zamowieniePlyta = await _context.ZamowieniePlyta.FindAsync(id);
            _context.ZamowieniePlyta.Remove(zamowieniePlyta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZamowieniePlytaExists(int id)
        {
            return _context.ZamowieniePlyta.Any(e => e.ZamowienieId == id);
        }

        private void loadDDL()
        {
            try
            {
                List<Zamowienie>zamowienieList = new List<Zamowienie>();
                zamowienieList = _context.Zamowienie.ToList();
                // addressList.Insert(0, new Adres { AdresId = 0, Ulica = "Proszę wybrać" ,Miejscowosc = "" });

                var adress = _context.Adres
                        .Select(s => new
                        {
                            Text = s.Miejscowosc + " " + s.Ulica + " " + s.NrDomu + " " + s.NrLokalu + " " + s.KodPocztowy,
                            Value = s.AdresId
                        })
                        .ToList();

               // ViewBag.AddressList = new SelectList(adress, "Value", "Text");

                ViewBag.ZamowienieList = zamowienieList;

            }
            catch (Exception ex)
            {


            }
        }
    }
}
