using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirmaMeblarska.Data;
using FirmaMeblarska.Models;
using Microsoft.AspNetCore.Authorization;

namespace FirmaMeblarska.Controllers
{
    public class NarzedzieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NarzedzieController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Narzedzie
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                //var pracList = _Db.Pracownik.ToList();

                var pracList = from a in _context.Narzedzie
                               join b in _context.Pracownik
                               on a.PracownikId equals b.PracownikId
                               into Pracownikss
                               from b in Pracownikss.DefaultIfEmpty()

                               select new Narzedzie
                               {
                                   NarzedzieId = a.NarzedzieId,
                                   Nazwa = a.Nazwa,
                                   NrSeryjny = a.NrSeryjny,
                                   PracownikId = a.PracownikId,                                   
                                   Pracowniks = b == null ? " " : b.Imie + " " + b.Nazwisko
                               };

                return View(pracList);
            }
            catch (Exception)
            {
                return View(await _context.Narzedzie.ToListAsync());

            }
            //var applicationDbContext = _context.Narzedzie.Include(n => n.Pracownik);
            //return View(await applicationDbContext.ToListAsync());
        }




        // GET: Narzedzie/Create
        [Authorize(Roles = "Admin")]
        public IActionResult AddOrEdit(int id = 0)
        {
            loadDDL();
            if (id == 0)
                return View(new Narzedzie());
            else
                return View(_context.Narzedzie.Find(id));
            //ViewData["PracownikId"] = new SelectList(_context.Pracownik, "PracownikId", "Email");
        }


        // POST: Narzedzie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("NarzedzieId,Nazwa,NrSeryjny,PracownikId")] Narzedzie narzedzie)
        {

            if (ModelState.IsValid)
            {
                if (narzedzie.NarzedzieId == 0)
                    _context.Add(narzedzie);

                else
                    _context.Update(narzedzie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            return View(narzedzie);

            /*if (ModelState.IsValid)
            {
                _context.Add(narzedzie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PracownikId"] = new SelectList(_context.Pracownik, "PracownikId", "Email", narzedzie.PracownikId);
            return View(narzedzie);*/
        }

        // GET: Narzedzie/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var narzedzie = await _context.Narzedzie.FindAsync(id);
            if (narzedzie == null)
            {
                return NotFound();
            }
            ViewData["PracownikId"] = new SelectList(_context.Pracownik, "PracownikId", "Email", narzedzie.PracownikId);
            return View(narzedzie);
        }

        // POST: Narzedzie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NarzedzieId,Nazwa,NrSeryjny,PracownikId")] Narzedzie narzedzie)
        {
            if (id != narzedzie.NarzedzieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(narzedzie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NarzedzieExists(narzedzie.NarzedzieId))
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
            ViewData["PracownikId"] = new SelectList(_context.Pracownik, "PracownikId", "Email", narzedzie.PracownikId);
            return View(narzedzie);
        }

        // GET: Narzedzie/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var czesc = await _context.Narzedzie.FindAsync(id);
            _context.Narzedzie.Remove(czesc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
            catch (Exception /* dex */)
            {
                ModelState.AddModelError("", "Dane nie zostały usunięte.");
                TempData["SuccessMessage"] = "Dane nie zostały usunięte, powieważ są powiązane z innym obiektem!";
                return RedirectToAction(nameof(Index));
    }
}

        private bool NarzedzieExists(int id)
        {
            return _context.Narzedzie.Any(e => e.NarzedzieId == id);
        }

        private void loadDDL()
        {
            try
            {
                List<Pracownik> pracList = new List<Pracownik>();
                var pracc = _context.Pracownik
                        .Select(s => new
                        {
                            Text = s.Imie + " " + s.Nazwisko,
                            Value = s.PracownikId
                        })
                        .ToList();

                ViewBag.PracList = new SelectList(pracc, "Value", "Text");

                //ViewBag.AddressList = addressList;

            }
            catch (Exception ex)
            {


            }
        }
    }
}
