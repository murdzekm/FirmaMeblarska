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
            try
            {
                //var pracList = _Db.Pracownik.ToList();

                var pracList = from a in _context.Klient
                               join b in _context.Adres
                               on a.AdresId equals b.AdresId
                               into Address
                               from b in Address.DefaultIfEmpty()

                               select new Klient
                               {
                                   KlientId = a.KlientId,
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
                return View(await _context.Klient.ToListAsync());

            }
            //return View(await _context.Klient.ToListAsync());
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
                //addressList = _context.Adres.ToList();
                // addressList.Insert(0, new Adres { AdresId = 0, Ulica = "Proszę wybrać" ,Miejscowosc = "" });

                var adress = _context.Adres
                        .Select(s => new
                        {
                            Text = s.Miejscowosc + " " + s.Ulica + " " + s.NrDomu + " " + s.NrLokalu + " " + s.KodPocztowy,
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