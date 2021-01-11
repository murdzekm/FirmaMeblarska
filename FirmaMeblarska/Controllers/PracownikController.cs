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
            try
            {
                //var pracList = _Db.Pracownik.ToList();

                var pracList = from a in _context.Pracownik
                               join b in _context.Adres
                               on a.AdresId equals b.AdresId
                               into Address
                               from b in Address.DefaultIfEmpty()

                               select new Pracownik
                               {
                                   PracownikId = a.PracownikId,
                                   Imie = a.Imie,
                                   Nazwisko = a.Nazwisko,
                                   Email = a.Email,
                                   Telefon = a.Telefon,
                                   AdresId = a.AdresId,
                                   Adress = b == null ? "" : b.Miejscowosc + " " + b.Ulica + " " + b.NrDomu + " " + b.KodPocztowy
                               };

                return View(pracList);
            }
            catch (Exception)
            {
                return View(await _context.Pracownik.ToListAsync());

            }
            // return View(await _context.Pracownik.ToListAsync());
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
                /* if(_context.Adres.Any(o => o.Miejscowosc == obj.Miejscowosc && o.Ulica == obj.Ulica)){
                     lastestAdrId = a.AdresId;
                      _context.Adres.Update(a);
                     _context.SaveChanges();



                 }
                 else
                 {*/
                if (obj.NrLokalu == null)
                    a.NrLokalu = " ";
                if (obj.Ulica == null)
                    a.Ulica = " ";
                _context.Adres.Add(a);
                _context.SaveChanges();
               // }
                lastestAdrId = a.AdresId;

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
            return View(obj);

         }

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
            //ViewData["PlytaId"] = new SelectList(_context.Plyta, "PlytaId", "Kod", zamowieniePlyta.PlytaId);
            //ViewData["ZamowienieId"] = new SelectList(_context.Zamowienie, "ZamowienieId", "ZamowienieId", zamowieniePlyta.ZamowienieId);
            return View(obj);
        }


        /*
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prac = await _context.Pracownik
                .Include(p => p.Adres)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.PracownikId == id);
            
            //.FindAsync(id);
            if (prac == null)
            {
                return NotFound();
            }
            
            return View(prac);
        }

        // POST: Zespol/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ZespolId,Nazwa")] Zespol zespol,string[] selectedConditions)
        public async Task<IActionResult> Edit(int id, string[] selectedConditions)
        {
            var pracToUpdate = await _context.Pracownik
               .Include(p => p.Adres)               
               .SingleOrDefaultAsync(p => p.PracownikId == id);          
           


            if (pracToUpdate == null)
            {
                return NotFound();
            }

            //UpdateZespolPracownik(selectedConditions, pracToUpdate);

            if (await TryUpdateModelAsync<Pracownik>(pracToUpdate, "",
                p => p.Imie, p => p.Nazwisko, p => p.Email, p => p.Telefon, p => p.AdresId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (RetryLimitExceededException /* dex *//*)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                                
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }

            }

            //PopulateAssignedConditionData(zespolToUpdate);
            PopulateDropDownLists(pracToUpdate);
            return View(pracToUpdate);

        }*/

        private void PopulateDropDownLists(Pracownik pracownik = null)
        {
            ViewData["DoctorID"] = DoctorSelectList(pracownik?.AdresId);
        }
        private SelectList DoctorSelectList(int? id)
        {
            var dQuery = from d in _context.Adres
                         orderby d.Miejscowosc, d.Ulica, d.NrDomu, d.KodPocztowy
                         select d;
            return new SelectList(dQuery, "ID", "FormalName", id);
        }


        // GET: Pracownik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var czesc = await _context.Pracownik.FindAsync(id);
            _context.Pracownik.Remove(czesc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void loadDDL()
        {
            try
            {
                List<Adres> addressList = new List<Adres>();
                //addressList = _context.Adres.ToList();
                // addressList.Insert(0, new Adres { AdresId = 0, Ulica = "Proszę wybrać" ,Miejscowosc = "" });

                var adress = _context.Adres
                        .Select(s => new
                        {                         
                            Text = s.Miejscowosc + " " + s.Ulica + " " + s.NrDomu+ " " + s.NrLokalu + " " + s.KodPocztowy,
                            Value = s.AdresId
                        })
                        .ToList();
                
                ViewBag.AddressList = new SelectList(adress, "Value", "Text");

                //ViewBag.AddressList = addressList;

            }
            catch (Exception ex)
            {


            }
        }


    }
}
