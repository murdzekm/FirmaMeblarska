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
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Policy = "writepolicy")]
        public async Task<IActionResult> Index()
        {

            var zamowienie = _context.Zamowienie
                   .Include(c => c.Klient)
                   .Include(c => c.Status)
                   .Include(c => c.Adres)
                   .Include(c => c.Zespol);

            return View(await zamowienie.ToListAsync());


        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie = await _context.Zamowienie
                    .Include(c => c.Klient)
                    .Include(c => c.Status)
                    .Include(c => c.Adres)
                    .Include(c => c.Zespol)
                    .SingleOrDefaultAsync(m => m.ZamowienieId.ToString() == id);
            if (zamowienie == null)
            {
                return NotFound();
            }

            return View(zamowienie);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            loadDDL();

            var zamowienie = new Zamowienie();
            zamowienie.ZamowieniePlyta = new List<ZamowieniePlyta>();
            ListaPlyt(zamowienie);
            Zamowienie obj = new Zamowienie();
            obj.DataZlozenia = DateTime.UtcNow.Date;
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZamowienieId,NrFaktura,DataZlozenia,Cena,StatusId,KlientId,ZespolId,AdresId")] Zamowienie zamowienie, string[] selectedplyty)

        {
            try
            {
                if (selectedplyty != null)
                {
                    zamowienie.ZamowieniePlyta = new List<ZamowieniePlyta>();
                    foreach (var cond in selectedplyty)
                    {
                        var condToAdd = new ZamowieniePlyta { ZamowienieId = zamowienie.ZamowienieId, PlytaId = int.Parse(cond) };
                        zamowienie.ZamowieniePlyta.Add(condToAdd);
                    }
                }

                var adress = _context.Klient.Where(m=>m.KlientId == zamowienie.KlientId).FirstOrDefault();
                zamowienie.AdresId = adress.AdresId;               
                if (ModelState.IsValid)
                {                    
                    _context.Add(zamowienie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Dane nie zostały dodane.");
            }
            ListaPlyt(zamowienie);
            return View(zamowienie);
        }

        // GET: Zespol/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            loadDDL();
            if (id == null)
            {
                return NotFound();
            }
            var zamowienie = await _context.Zamowienie
                .Include(p => p.ZamowieniePlyta).ThenInclude(p => p.Plyta)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.ZamowienieId == id);
            if (zamowienie == null)
            {
                return NotFound();
            }
            ListaPlyt(zamowienie);
            return View(zamowienie);
        }
        // POST: Zespol/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Edit(int id, string[] selectedplyty)
        {
            var zamowienieToUpdate = await _context.Zamowienie
                .Include(d => d.ZamowieniePlyta).ThenInclude(d => d.Plyta)
                .SingleOrDefaultAsync(d => d.ZamowienieId == id);          
            if (zamowienieToUpdate == null)
            {
                return NotFound();
            }

            UpdateZespolPracownik(selectedplyty, zamowienieToUpdate);
            if (await TryUpdateModelAsync<Zamowienie>(zamowienieToUpdate, "",
                d => d.NrFaktura, d => d.Cena, d => d.DataZlozenia, d => d.KlientId, d => d.AdresId, d => d.ZespolId, d => d.StatusId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Dane nie zostały zaaktualizowane.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZamowienieExists(zamowienieToUpdate.ZamowienieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Dane nie zostały zaaktualizowane.");
                }

            }
            ListaPlyt(zamowienieToUpdate);
            return View(zamowienieToUpdate);

        }


        private void ListaPlyt(Zamowienie zamowienie)
        {
            var allplyty = _context.Plyta;
            var pplyty = new HashSet<int>(zamowienie.ZamowieniePlyta.Select(b => b.PlytaId));
            var viewModel = new List<PlytaVW>();
            foreach (var con in allplyty)
            {
                viewModel.Add(new PlytaVW
                {
                    PlytaId = con.PlytaId,
                    Nazwa = con.Kod,
                    Assigned = pplyty.Contains(con.PlytaId)
                });
            }
            ViewData["Plyty"] = viewModel;
        }
        private void UpdateZespolPracownik(string[] selectedPlyty, Zamowienie zamowienieToUpdate)
        {
            if (selectedPlyty == null)
            {
                zamowienieToUpdate.ZamowieniePlyta = new List<ZamowieniePlyta>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedPlyty);
            var zamPlyta = new HashSet<int>(zamowienieToUpdate.ZamowieniePlyta.Select(b => b.PlytaId));
            foreach (var s in _context.Plyta)
            {
                if (selectedOptionsHS.Contains(s.PlytaId.ToString()))
                {
                    if (!zamPlyta.Contains(s.PlytaId))
                    {
                        zamowienieToUpdate.ZamowieniePlyta.Add(new ZamowieniePlyta
                        {
                            PlytaId = s.PlytaId,
                            ZamowienieId = zamowienieToUpdate.ZamowienieId
                        });
                    }
                }
                else
                {
                    if (zamPlyta.Contains(s.PlytaId))
                    {
                        ZamowieniePlyta plytaToRemove = zamowienieToUpdate.ZamowieniePlyta.SingleOrDefault(d => d.PlytaId == s.PlytaId);
                        _context.Remove(plytaToRemove);
                    }
                }
            }
        }

        public async Task<IActionResult> ShowInvoice(string id)
        {
            Zamowienie obj = await _context.Zamowienie
                .Include(c => c.Klient)
                    .Include(c => c.Status)
                    .Include(c => c.Adres)
                    .Include(c => c.Zespol)
                    .SingleOrDefaultAsync(m => m.ZamowienieId.ToString() == id);           

            return View(obj);
        }

        public async Task<IActionResult> PrintInvoice(string id)
        {
            Zamowienie obj = await _context.Zamowienie
                .Include(c => c.Klient)
                    .Include(c => c.Status)
                    .Include(c => c.Adres)
                    .Include(c => c.Zespol)
                    .SingleOrDefaultAsync(m => m.ZamowienieId.ToString() == id);

            return View(obj);
        }


        private bool ZamowienieExists(int id)
        {
            return _context.Zamowienie.Any(e => e.ZamowienieId == id);
        }

        /*
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
        public async Task<IActionResult> AddOrEdit([Bind("ZamowienieId,DataZlozenia,Cena,StatusId,KlientId,ZespolId,AdresId")] Zamowienie zamowienie)
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
        */




        // GET: Zamowienie/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {   
                var czesc = await _context.Zamowienie.FindAsync(id);
                _context.Zamowienie.Remove(czesc);
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
                List<Klient> klientList = new List<Klient>();                
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

                List<Adres> AdresList = new List<Adres>();
                var adress = _context.Adres
                        .Select(s => new
                        {
                            Text = s.FullAdres,
                            Value = s.AdresId
                        })
                        .ToList();
                ViewBag.AdresList = new SelectList(adress, "Value", "Text");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
