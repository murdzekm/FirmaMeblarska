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
    public class ZamowienieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZamowienieController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Zamowienie
        public async Task<IActionResult> Index()
        {
            try
            {
                //var pracList = _Db.Pracownik.ToList();

                var pracList = from a in _context.Zamowienie
                               join b in _context.Klient                                
                               on a.KlientId equals b.KlientId
                               join c in _context.Zespol
                               on a.ZespolId equals c.ZespolId
                               join d in _context.Status
                               on a.StatusId equals d.StatusId
                               into ZamoKli
                               from d in ZamoKli.DefaultIfEmpty()

                               select new Zamowienie
                               {
                                   ZamowienieId = a.ZamowienieId,
                                   DataZlozenia = a.DataZlozenia,
                                   Cena = a.Cena,
                                   StatusId = a.StatusId,                                   
                                   Statuss = d == null ? "" : d.Nazwa,                                   
                                   KlientId = a.KlientId,
                                   Klients = b == null ? "" : b.Imie + " " + b.Nazwisko,
                                   ZespolId = a.ZespolId,
                                   Zespols = c == null ? "" : c.Nazwa,


                               };

                return View(pracList);
            }
            catch (Exception)
            {
                return View(await _context.Zamowienie.ToListAsync());

            }
           
        }

        

        // GET: Zamowienie/Create
        public IActionResult AddOrEdit(int id = 0)
        {
           
            loadDDL();
            if (id == 0)
                return View(new Zamowienie());
            else
                return View(_context.Zamowienie.Find(id));
        }

        // POST: Zamowienie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("ZamowienieId,DataZlozenia,Cena,StatusId,KlientId,ZespolId")] Zamowienie zamowienie)
        {
            if (ModelState.IsValid)
            {
                if (zamowienie.ZamowienieId == 0)
                    _context.Add(zamowienie);

                else
                    _context.Update(zamowienie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            return View(zamowienie);
        }

       

        

        // GET: Zamowienie/Delete/5
       
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
                List<Klient> klientList = new List<Klient>();
                //addressList = _context.Adres.ToList();
                // addressList.Insert(0, new Adres { AdresId = 0, Ulica = "Proszę wybrać" ,Miejscowosc = "" });

                var klients = _context.Klient
                        .Select(s => new
                        {
                            Text = s.Imie + " " + s.Nazwisko ,
                            Value = s.KlientId
                        })
                        .ToList();

                ViewBag.KlientList = new SelectList(klients, "Value", "Text");

                List<Status> StatusList = new List<Status>();
                

                var statuss = _context.Status
                        .Select(s => new
                        {
                            Text = s.Nazwa ,
                            Value = s.StatusId
                        })
                        .ToList();

                ViewBag.StatusList = new SelectList(statuss, "Value", "Text");


                List<Zespol> ZespolList = new List<Zespol>();


                var zespols = _context.Zespol
                        .Select(s => new
                        {
                            Text = s.Nazwa,
                            Value = s.ZespolId
                        })
                        .ToList();

                ViewBag.ZespolList = new SelectList(zespols, "Value", "Text");

            }
            catch (Exception ex)
            {


            }
        }

    }
}
