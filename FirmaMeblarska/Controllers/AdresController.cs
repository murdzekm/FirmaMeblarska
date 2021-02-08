using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirmaMeblarska.Models;
using FirmaMeblarska.Data;
using Microsoft.AspNetCore.Authorization;

namespace FirmaMeblarska.Controllers
{
    public class AdresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdresController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Adres
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Adres.ToListAsync());
        }



        // GET: Adres/Create
        [Authorize(Roles = "Admin")]
        public IActionResult AddOrEdit(int id = 0)
        {
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            if (id == 0)
                return View(new Adres());
            else
                return View(_context.Adres.Find(id));
        }

        // POST: Magazyn/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("AdresId,Miejscowosc,Ulica,NrDomu,NrLokalu,KodPocztowy")] Adres adres, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (adres.AdresId == 0)
                {                   
                    _context.Add(adres);
                }
                else
                {
                    _context.Update(adres);
                }
                 await _context.SaveChangesAsync();
               
                return RedirectToAction(nameof(Index));
                //return Redirect(returnUrl);

            }
            return View(adres);
        }


        // GET: Adres/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            try { 
            var czesc = await _context.Adres.FindAsync(id);
            _context.Adres.Remove(czesc);
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

    }
}
