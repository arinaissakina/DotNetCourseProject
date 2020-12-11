using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetFinalProject.Data;
using DotNetFinalProject.Models;
using DotNetFinalProject.Services;
using Microsoft.AspNetCore.Authorization;

namespace DotNetFinalProject.Controllers
{
    [Authorize(Roles="ADMIN")]
    public class SpecialtyController : Controller
    {
        private readonly SpecialtyService _specialtyService;

        public SpecialtyController(SpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        // GET: Specialty
        public async Task<IActionResult> Index()
        {
            return View(await _specialtyService.GetSpecialties());
        }

        // GET: Specialty/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await _specialtyService.GetSpecialty(id);
            if (specialty == null)
            {
                return NotFound();
            }

            return View(specialty);
        }

        // GET: Specialty/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Specialty/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Specialty specialty)
        {
            if (ModelState.IsValid)
            {
                await _specialtyService.CreateSpecialty(specialty);
                return RedirectToAction(nameof(Index));
            }
            return View(specialty);
        }

        // GET: Specialty/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await _specialtyService.GetSpecialty(id);
            if (specialty == null)
            {
                return NotFound();
            }
            return View(specialty);
        }

        // POST: Specialty/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name")] Specialty specialty)
        {
            if (id != specialty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _specialtyService.EditSpecialty(specialty);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialtyExists(specialty.Id))
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
            return View(specialty);
        }

        // GET: Specialty/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await _specialtyService.GetSpecialty(id);
            if (specialty == null)
            {
                return NotFound();
            }

            return View(specialty);
        }

        // POST: Specialty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var specialty = await _specialtyService.GetSpecialty(id);
            await _specialtyService.DeleteSpecialty(specialty);
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialtyExists(long id)
        {
            return _specialtyService.SpecialtyExist(id);
        }
    }
}
