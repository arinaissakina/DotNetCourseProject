using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetFinalProject.Data;
using DotNetFinalProject.Models;

namespace CourseProject.Controllers
{
    public class SpecialtyUserController : Controller
    {
        private readonly CourseProjectContext _context;

        public SpecialtyUserController(CourseProjectContext context)
        {
            _context = context;
        }

        // GET: SpecialtyUser
        public async Task<IActionResult> Index()
        {
            var courseProjectContext = _context.SpecialityUsers.Include(s => s.Specialty).Include(s => s.User);
            return View(await courseProjectContext.ToListAsync());
        }

        // GET: SpecialtyUser/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialtyUser = await _context.SpecialityUsers
                .Include(s => s.Specialty)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SpecialtyId == id);
            if (specialtyUser == null)
            {
                return NotFound();
            }

            return View(specialtyUser);
        }

        // GET: SpecialtyUser/Create
        public IActionResult Create()
        {
            ViewData["SpecialtyId"] = new SelectList(_context.Specialties, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.AllUsers, "Id", "Id");
            return View();
        }

        // POST: SpecialtyUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpecialtyId,UserId")] SpecialtyUser specialtyUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specialtyUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecialtyId"] = new SelectList(_context.Specialties, "Id", "Name", specialtyUser.SpecialtyId);
            ViewData["UserId"] = new SelectList(_context.AllUsers, "Id", "Name", specialtyUser.UserId);
            return View(specialtyUser);
        }

        // GET: SpecialtyUser/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialtyUser = await _context.SpecialityUsers.FindAsync(id);
            if (specialtyUser == null)
            {
                return NotFound();
            }
            ViewData["SpecialtyId"] = new SelectList(_context.Specialties, "Id", "Name", specialtyUser.SpecialtyId);
            ViewData["UserId"] = new SelectList(_context.AllUsers, "Id", "Id", specialtyUser.UserId);
            return View(specialtyUser);
        }

        // POST: SpecialtyUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SpecialtyId,UserId")] SpecialtyUser specialtyUser)
        {
            if (id != specialtyUser.SpecialtyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specialtyUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialtyUserExists(specialtyUser.SpecialtyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecialtyId"] = new SelectList(_context.Specialties, "Id", "Name", specialtyUser.SpecialtyId);
            ViewData["UserId"] = new SelectList(_context.AllUsers, "Id", "Id", specialtyUser.UserId);
            return View(specialtyUser);
        }

        // GET: SpecialtyUser/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialtyUser = await _context.SpecialityUsers
                .Include(s => s.Specialty)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SpecialtyId == id);
            if (specialtyUser == null)
            {
                return NotFound();
            }

            return View(specialtyUser);
        }

        // POST: SpecialtyUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var specialtyUser = await _context.SpecialityUsers.FindAsync(id);
            _context.SpecialityUsers.Remove(specialtyUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialtyUserExists(long id)
        {
            return _context.SpecialityUsers.Any(e => e.SpecialtyId == id);
        }
    }
}
