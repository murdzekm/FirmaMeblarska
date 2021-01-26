using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirmaMeblarska.Models;
using FirmaMeblarska.Data;
using FirmaMeblarska.ViewModels;

namespace FirmaMeblarska.Controllers
{
    public class KlientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KlientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Klient
        public async Task<IActionResult> Index()
        {

            var klient = _context.Klient
                 .Include(c => c.Adres);

            return View(await klient.ToListAsync());
           
        }


        // GET: Klient/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            loadDDL();
            if (id == 0)
                return View(new Klient());
            else
                return View(_context.Klient.Find(id));
        }

        // POST: Klient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("KlientId,Imie,Nazwisko,Email,Telefon,AdresId")] Klient klient)
        {
            if (ModelState.IsValid)
            {
                if (klient.KlientId == 0)
                    _context.Add(klient);

                else
                    _context.Update(klient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            return View(klient);
        }


        [HttpGet]
        public ActionResult AddKlientAdres()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddKlientAdres(AdresKlientVW obj)
        {
            try
            {
                int lastestAdrId;
                Adres a = new Adres();
                a.Miejscowosc = obj.Miejscowosc;
                a.Ulica = obj.Ulica;
                a.NrDomu = obj.NrDomu;
                a.NrLokalu = obj.NrLokalu;
                a.KodPocztowy = obj.KodPocztowy;
                var adress = _context.Adres.Where(m => m.Miejscowosc == obj.Miejscowosc && m.Ulica == obj.Ulica && m.NrDomu == obj.NrDomu && m.NrLokalu == obj.NrLokalu).FirstOrDefault();

                if (adress == null)
                {
                    lastestAdrId = a.AdresId;
                    _context.Adres.Add(a);
                    _context.SaveChanges();
                }
                else
                {
                    lastestAdrId = adress.AdresId;
                }                

                Klient p = new Klient();
                p.Imie = obj.Imie;
                p.Nazwisko = obj.Nazwisko;
                p.Email = obj.Email;
                p.Telefon = obj.Telefon;
                p.AdresId = lastestAdrId;

                _context.Klient.Add(p);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return RedirectToAction(nameof(Index));

        }



        // GET: Klient/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var czesc = await _context.Klient.FindAsync(id);
            _context.Klient.Remove(czesc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void loadDDL()
        {
            try
            {
                List<Adres> addressList = new List<Adres>();            
                var adress = _context.Adres
                        .Select(s => new
                        {
                            Text = s.FullAdres,
                            Value = s.AdresId
                        })
                        .ToList();

                ViewBag.AddressList = new SelectList(adress, "Value", "Text");
            }
            catch (Exception ex)
            {

            }
        }
    }
}