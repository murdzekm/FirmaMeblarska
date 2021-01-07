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
                                   Adres = b == null ? "" : b.Miejscowosc + " " + b.Ulica + " " + b.NrDomu + " " + b.KodPocztowy

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
