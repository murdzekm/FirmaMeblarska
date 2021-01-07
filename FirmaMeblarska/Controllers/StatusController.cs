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
    public class StatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Status
        public async Task<IActionResult> Index()
        {
            return View(await _context.Status.ToListAsync());
        }

        

        // GET: Status/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Status());
            else
                return View(_context.Status.Find(id));
        }

       

       

        // POST: Status/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("StatusId,Nazwa")] Status status)
        {
            if (ModelState.IsValid)
            {
                if (status.StatusId == 0)
                    _context.Add(status);

                else
                    _context.Update(status);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            return View(status);
        }

        // GET: Status/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var czesc = await _context.Status.FindAsync(id);
            _context.Status.Remove(czesc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
