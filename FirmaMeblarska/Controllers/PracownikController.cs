using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirmaMeblarska.Models;
using Microsoft.AspNetCore.Authorization;
using FirmaMeblarska.Data;
using FirmaMeblarska.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;

namespace FirmaMeblarska.Controllers
{
    public class PracownikController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PracownikController(ApplicationDbContext context)
        {
            _context = context;
        }

// GET: Pracownik
        [Authorize(Roles = "Admin")]        
        public async Task<IActionResult> Index()
        {
            var pracownik = _context.Pracownik
                  .Include(c => c.Adres);

            return View(await pracownik.ToListAsync());
           
        }



        // GET: Pracownik/Create
        [Authorize(Roles = "Admin")]
        public IActionResult AddOrEdit(int id = 0)
        {

            List<Adres> adressList = _context.Adres.ToList();
            loadDDL();
            if (id == 0)
                return View(new Pracownik());
            else
                return View(_context.Pracownik.Find(id));
        }

        // POST: Pracownik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("PracownikId,Imie,Nazwisko,Email,Telefon,AdresId")] Pracownik pracownik)
        {
            if (ModelState.IsValid)
            {
                if (pracownik.PracownikId == 0)
                    _context.Add(pracownik);

                else
                    _context.Update(pracownik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            return View(pracownik);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AddPracownikAdres()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPracownikAdres([Bind("PracownikId,Imie,Nazwisko,Email,Telefon,AdresId,Miejscowosc,Ulica,NrDomu,NrLokalu,KodPocztowy")] AdresVW obj)
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
                
                 if(adress == null){
                     lastestAdrId = a.AdresId;
                    _context.Adres.Add(a);
                    _context.SaveChanges();
                    


                 }
                 else
                 {
                    lastestAdrId = adress.AdresId;
                    //_context.Adres.Update(a);
                    //_context.SaveChanges();
                  }
               
                Pracownik p = new Pracownik();
                p.Imie = obj.Imie;
                p.Nazwisko = obj.Nazwisko;
                p.Email = obj.Email;
                p.Telefon = obj.Telefon;
                p.AdresId = lastestAdrId;

                _context.Pracownik.Add(p);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return RedirectToAction(nameof(Index));
            //return View(obj);

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prac = await _context.Pracownik.FindAsync(id);
            if (prac == null)
            {
                return NotFound();
            }
         //   ViewData["PlytaId"] = new SelectList(_context.Plyta, "PlytaId", "Kod", zamowieniePlyta.PlytaId);
          //  ViewData["ZamowienieId"] = new SelectList(_context.Zamowienie, "ZamowienieId", "ZamowienieId", zamowieniePlyta.ZamowienieId);
            return View(prac);
        }

        // POST: ZamowieniePlyta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PracownikId,Imie,Nazwisko,Email,Telefon,AdresId,Miejscowosc,Ulica,NrDomu,NrLokalu,KodPocztowy")] AdresVW obj)
        {
            if (id != obj.PracownikId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Adres a = new Adres();
                    a.Miejscowosc = obj.Miejscowosc;
                    a.Ulica = obj.Ulica;
                    a.NrDomu = obj.NrDomu;
                    a.NrLokalu = obj.NrLokalu;
                    a.KodPocztowy = obj.KodPocztowy;

                    if (obj.NrLokalu == null)
                        obj.NrLokalu = " ";
                    if (obj.Ulica == null)
                        obj.Ulica = " ";

                    _context.Adres.Update(a);
                    await _context.SaveChangesAsync();

                    int lastestAdrId = a.AdresId;

                    Pracownik p = new Pracownik();
                    p.Imie = obj.Imie;
                    p.Nazwisko = obj.Nazwisko;
                    p.Email = obj.Email;
                    p.Telefon = obj.Telefon;
                    p.AdresId = lastestAdrId;

                    _context.Pracownik.Update(p);
                    await _context.SaveChangesAsync();
                  //  _context.Update(zamowieniePlyta);
                   // await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                        throw;
                    
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(obj);
        }

       
        // GET: Pracownik/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var czesc = await _context.Pracownik.FindAsync(id);
            _context.Pracownik.Remove(czesc);
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
