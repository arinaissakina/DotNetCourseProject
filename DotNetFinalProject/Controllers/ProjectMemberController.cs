using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetFinalProject.Data;
using DotNetFinalProject.Models;

namespace DotNetFinalProject.Controllers
{
    public class ProjectMemberController : Controller
    {
        private readonly CourseProjectContext _context;

        public ProjectMemberController(CourseProjectContext context)
        {
            _context = context;
        }

        // GET: ProjectMember
        public async Task<IActionResult> Index()
        {
            var courseProjectContext = _context.ProjectMembers.Include(p => p.Member).Include(p => p.Project);
            return View(await courseProjectContext.ToListAsync());
        }

        // GET: ProjectMember/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectMember = await _context.ProjectMembers
                .Include(p => p.Member)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (projectMember == null)
            {
                return NotFound();
            }

            return View(projectMember);
        }

        // GET: ProjectMember/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.AllUsers, "Id", "Name");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Description");
            return View();
        }

        // POST: ProjectMember/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,MemberId")] ProjectMember projectMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.AllUsers, "Id", "Name", projectMember.MemberId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Description", projectMember.ProjectId);
            return View(projectMember);
        }

        // GET: ProjectMember/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectMember = await _context.ProjectMembers.FindAsync(id);
            if (projectMember == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.AllUsers, "Id", "Name", projectMember.MemberId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Description", projectMember.ProjectId);
            return View(projectMember);
        }

        // POST: ProjectMember/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ProjectId,MemberId")] ProjectMember projectMember)
        {
            if (id != projectMember.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectMemberExists(projectMember.ProjectId))
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
            ViewData["MemberId"] = new SelectList(_context.AllUsers, "Id", "Name", projectMember.MemberId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Description", projectMember.ProjectId);
            return View(projectMember);
        }

        // GET: ProjectMember/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectMember = await _context.ProjectMembers
                .Include(p => p.Member)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (projectMember == null)
            {
                return NotFound();
            }

            return View(projectMember);
        }

        // POST: ProjectMember/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var projectMember = await _context.ProjectMembers.FindAsync(id);
            _context.ProjectMembers.Remove(projectMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectMemberExists(long id)
        {
            return _context.ProjectMembers.Any(e => e.ProjectId == id);
        }
    }
}
