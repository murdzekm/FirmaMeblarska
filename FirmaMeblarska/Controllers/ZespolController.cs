﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirmaMeblarska.Data;
using FirmaMeblarska.Models;
using FirmaMeblarska.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;

namespace FirmaMeblarska.Controllers
{
    public class ZespolController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZespolController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Zespol
        public async Task<IActionResult> Index()
        {
            return View(await _context.Zespol.ToListAsync());
        }

        // GET: Zespol/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zespol = await _context.Zespol                
                .FirstOrDefaultAsync(m => m.ZespolId == id);
            if (zespol == null)
            {
                return NotFound();
            }

            return View(zespol);
        }

        // GET: Zespol/Create
        public IActionResult Create()
        {
            var zespol = new Zespol();
            zespol.ZespolPracownik = new List<ZespolPracownik>();
            PopulateAssignedConditionData(zespol);         
            return View();
        }

        // POST: Zespol/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZespolId,Nazwa")] Zespol zespol, string[] selectedConditions)
        {
            try
            {
                UpdateZespolPracownik(selectedConditions, zespol);
                if (ModelState.IsValid)
                {
                    _context.Add(zespol);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            PopulateAssignedConditionData(zespol);
            return View(zespol);
        
            
        }

        // GET: Zespol/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zespol = await _context.Zespol
                .Include(p => p.ZespolPracownik).ThenInclude(p => p.Pracowniks)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.ZespolId == id);
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

            var zespolToUpdate = await _context.Zespol
                .Include(d => d.ZespolPracownik).ThenInclude(d => d.Pracowniks)
                .SingleOrDefaultAsync(d => d.ZespolId == id);
            //Check that you got it or exit with a not found error
                       

            if (zespolToUpdate == null)
            {
                return NotFound();
            }

            UpdateZespolPracownik(selectedConditions, zespolToUpdate);

            if (await TryUpdateModelAsync<Zespol>(zespolToUpdate, "",
                d => d.Nazwa))
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
                    if (!ZespolExists(zespolToUpdate.ZespolId))
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

            var zespol = await _context.Zespol
                .FirstOrDefaultAsync(m => m.ZespolId == id);
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
            var zespol = await _context.Zespol.FindAsync(id);
            _context.Zespol.Remove(zespol);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZespolExists(int id)
        {
            return _context.Zespol.Any(e => e.ZespolId == id);
        }


        private void PopulateAssignedConditionData(Zespol zespol)
        {
            var allConditions = _context.Pracownik;
            var pConditions = new HashSet<int>(zespol.ZespolPracownik.Select(b => b.PracownikId));
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
        private void UpdateZespolPracownik(string[] selectedConditions, Zespol zespolToUpdate)
        {
            if (selectedConditions == null)
            {
                zespolToUpdate.ZespolPracownik = new List<ZespolPracownik>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedConditions);
            var docSpecialties = new HashSet<int>(zespolToUpdate.ZespolPracownik.Select(b => b.PracownikId));
            foreach (var s in _context.Pracownik)
            {
                if (selectedOptionsHS.Contains(s.PracownikId.ToString()))
                {
                    if (!docSpecialties.Contains(s.PracownikId))
                    {
                        zespolToUpdate.ZespolPracownik.Add(new ZespolPracownik
                        {
                            PracownikId = s.PracownikId,
                            ZespolId = zespolToUpdate.ZespolId
                        });
                    }
                }
                else
                {
                    if (docSpecialties.Contains(s.PracownikId))
                    {
                        ZespolPracownik specToRemove = zespolToUpdate.ZespolPracownik.SingleOrDefault(d => d.PracownikId == s.PracownikId);
                        _context.Remove(specToRemove);
                    }
                }
            }
        }

        public PartialViewResult ListOfSpecialtiesDetails(int id)
        {
            var query = from s in _context.ZespolPracownik.Include(p => p.Pracowniks)
                        where s.ZespolId == id
                        select s;
            return PartialView("_ListOfSpecialities", query.ToList());
        }
        /*private void PopulateSpecialtyDropDownList()
        {
            var dQuery = from d in _context.Specialties
                         orderby d.SpecialtyName
                         select d;
            ViewData["SpecialtyID"] = new SelectList(dQuery, "ID", "SpecialtyName");
        }*/
    }
}
