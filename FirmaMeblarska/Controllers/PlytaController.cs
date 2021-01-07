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
    public class PlytaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlytaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Plyta
        public async Task<IActionResult> Index()
        {
            return View(await _context.Plyta.ToListAsync());
        }

       

        // GET: Plyta/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Plyta());
            else
                return View(_context.Plyta.Find(id));
        }

        // POST: Plyta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("PlytaId,Kod,Ilosc")] Plyta plyta)
        {
            if (ModelState.IsValid)
            {
                if (plyta.PlytaId == 0)
                    _context.Add(plyta);

                else
                    _context.Update(plyta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            return View(plyta);
        }

                        

        // GET: Plyta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var czesc = await _context.Status.FindAsync(id);
            _context.Status.Remove(czesc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}
