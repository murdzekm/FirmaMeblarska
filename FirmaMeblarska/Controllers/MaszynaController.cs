using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirmaMeblarska.Data;
using FirmaMeblarska.Models;
using FirmaMeblarska.Utilities;
using FirmaMeblarska.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;

namespace FirmaMeblarska.Controllers
{
    public class MaszynaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaszynaController(ApplicationDbContext context)
        {
            _context = context;
        }
        //public async Task<IActionResult> Create([Bind("MaszynaId,Nazwa,NrSeryjny,DataPrzegladu")] Maszyna maszyna, string[] selectedConditions)
        public async Task<IActionResult> Index(int? page)
        {
            var zespol = from d in _context.Maszyna
                .Include(d => d.PracownikMaszyna).ThenInclude(d => d.Pracowniks)
                         select d;

            int pageSize = 999999999;//Change as required
            var pagedData = await PaginatedList<Maszyna>.CreateAsync(zespol.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
            //return View(await _context.Maszyna.ToListAsync());
        }




        // GET: Zespol/Create
        public IActionResult Create()
        {
            var maszyna = new Maszyna();
            maszyna.PracownikMaszyna = new List<PracownikMaszyna>();
            PopulateAssignedConditionData(maszyna);
            return View();
        }

        // POST: Zespol/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaszynaId,Nazwa,NrSeryjny,DataPrzegladu")] Maszyna maszyna, string[] selectedConditions)
        {
            try
            {
                if (selectedConditions != null)
                {
                    maszyna.PracownikMaszyna = new List<PracownikMaszyna>();
                    foreach (var cond in selectedConditions)
                    {
                        var condToAdd = new PracownikMaszyna { MaszynaId = maszyna.MaszynaId, PracownikId = int.Parse(cond) };
                        maszyna.PracownikMaszyna.Add(condToAdd);
                    }
                }

                //UpdateZespolPracownik(selectedConditions, zespol);
                if (ModelState.IsValid)
                {
                    _context.Add(maszyna);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            PopulateAssignedConditionData(maszyna);
            return View(maszyna);


        }

        // GET: Zespol/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zespol = await _context.Maszyna
                .Include(p => p.PracownikMaszyna).ThenInclude(p => p.Pracowniks)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.MaszynaId == id);
            //.FindAsync(id);
            if (zespol == null)
            {
                return NotFound();
            }
            PopulateAssignedConditionData(zespol);
            return View(zespol);
        }

        // POST: Zespol/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ZespolId,Nazwa")] Zespol zespol,string[] selectedConditions)
        public async Task<IActionResult> Edit(int id, string[] selectedConditions)
        {

            var zespolToUpdate = await _context.Maszyna
                .Include(d => d.PracownikMaszyna).ThenInclude(d => d.Pracowniks)
                .SingleOrDefaultAsync(d => d.MaszynaId == id);
            //Check that you got it or exit with a not found error


            if (zespolToUpdate == null)
            {
                return NotFound();
            }

            UpdateZespolPracownik(selectedConditions, zespolToUpdate);

            if (await TryUpdateModelAsync<Maszyna>(zespolToUpdate, "",
                d => d.Nazwa, d => d.NrSeryjny, d => d.DataPrzegladu))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZespolExists(zespolToUpdate.MaszynaId))
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
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }

            }

            PopulateAssignedConditionData(zespolToUpdate);
            return View(zespolToUpdate);

        }

        // GET: Zespol/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zespol = await _context.Maszyna
                .FirstOrDefaultAsync(m => m.MaszynaId == id);
            if (zespol == null)
            {
                return NotFound();
            }

            return View(zespol);
        }

        // POST: Zespol/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zespol = await _context.Maszyna.FindAsync(id);
            _context.Maszyna.Remove(zespol);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZespolExists(int id)
        {
            return _context.Zespol.Any(e => e.ZespolId == id);
        }


        private void PopulateAssignedConditionData(Maszyna zespol)
        {
            var allConditions = _context.Pracownik;
            var pConditions = new HashSet<int>(zespol.PracownikMaszyna.Select(b => b.PracownikId));
            var viewModel = new List<AssignedConditionVM>();
            foreach (var con in allConditions)
            {
                viewModel.Add(new AssignedConditionVM
                {
                    PracownikID = con.PracownikId,
                    PracownikNazwa = con.FullName,
                    Assigned = pConditions.Contains(con.PracownikId)
                });
            }
            ViewData["Conditions"] = viewModel;
        }
        private void UpdateZespolPracownik(string[] selectedConditions, Maszyna maszynaToUpdate)
        {
            if (selectedConditions == null)
            {
                maszynaToUpdate.PracownikMaszyna = new List<PracownikMaszyna>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedConditions);
            var docSpecialties = new HashSet<int>(maszynaToUpdate.PracownikMaszyna.Select(b => b.PracownikId));
            foreach (var s in _context.Pracownik)
            {
                if (selectedOptionsHS.Contains(s.PracownikId.ToString()))
                {
                    if (!docSpecialties.Contains(s.PracownikId))
                    {
                        maszynaToUpdate.PracownikMaszyna.Add(new PracownikMaszyna
                        {
                            PracownikId = s.PracownikId,
                            MaszynaId = maszynaToUpdate.MaszynaId
                        });
                    }
                }
                else
                {
                    if (docSpecialties.Contains(s.PracownikId))
                    {
                        PracownikMaszyna specToRemove = maszynaToUpdate.PracownikMaszyna.SingleOrDefault(d => d.PracownikId == s.PracownikId);
                        _context.Remove(specToRemove);
                    }
                }
            }
        }


    }
}
