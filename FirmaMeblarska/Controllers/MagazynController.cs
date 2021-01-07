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
    public class MagazynController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MagazynController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Magazyn
        [Authorize(Roles = "Admin")]
        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Magazyn.ToListAsync());
        }

               

        // GET: Magazyn/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if(id == 0)
                return View(new Magazyn());
            else
                return View(_context.Magazyn.Find(id));
        }

        // POST: Magazyn/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("CzescId,Nazwa,Ilosc")] Magazyn magazyn)
        {
            if (ModelState.IsValid)
            {
                if(magazyn.CzescId == 0)                
                    _context.Add(magazyn);
               
                else               
                    _context.Update(magazyn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
                
            }
            return View(magazyn);
        }

     
        // GET: Magazyn/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var czesc = await _context.Magazyn.FindAsync(id);
            _context.Magazyn.Remove(czesc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
