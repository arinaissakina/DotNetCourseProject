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
using DotNetFinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis;
using Project = DotNetFinalProject.Models.Project;

namespace DotNetFinalProject.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService;
        private readonly CourseProjectContext _context;

        public ProjectController(CourseProjectContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        // GET: Project
        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetProjects();
            return View(projects);
        }
        
        public async Task<IActionResult> Search(string text)
        {
            var searchedProjects = await _projectService.Search(text);
            return View("Index", searchedProjects);
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectService.GetProject(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            //ViewData["OwnerId"] = new SelectList(_context.AllUsers, "Id", "Name");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectViewModel model)
        {
            
                var userEmail = User.Identity.Name;
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
                Project project = new Project()
                    {OwnerId = user.Id, Name = model.Project.Name, Description = model.Project.Description};
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                _context.ProjectMembers.Add(new ProjectMember {ProjectId = project.Id, MemberId = user.Id});
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Project");
        }
        

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectService.GetProject(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_context.AllUsers, "Id", "Name", project.OwnerId);
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description,OwnerId")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _projectService.EditProject(project);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            ViewData["OwnerId"] = new SelectList(_context.AllUsers, "Id", "Name", project.OwnerId);
            return View(project);
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectService.GetProject(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var project = await _projectService.GetProject(id);
            await _projectService.DeleteProject(project);
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(long id)
        {
            return _projectService.ProjectExist(id);
        }
        
        
        //[Authorize(Roles = "USER")]
        [HttpGet]
        [Route("/Project/JoinProject/{id}")]
        public async Task<IActionResult> JoinProject(long id)
        {
            var userEmail = User.Identity.Name;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            
            var project = await _context.Projects.FirstOrDefaultAsync(c => c.Id == id);
            var projectMembers = await _context.ProjectMembers.FirstOrDefaultAsync(b => b.MemberId == user.Id && b.ProjectId == id);
            
            if (projectMembers == null)
            {
                _context.ProjectMembers.Add(new ProjectMember { ProjectId = id, MemberId = user.Id });
                await _context.SaveChangesAsync();
            }
      
            return RedirectToAction("Index", "Project");
        }
        
        //[Authorize(Roles = "USER")]
        [HttpGet]
        [Route("/Project/QuitProject/{id}")]
        public async Task<IActionResult> QuitProject(long id)
        {
            var userEmail = User.Identity.Name;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            
            var project = await _context.Projects.FirstOrDefaultAsync(c => c.Id == id);
            var projectMembers = await _context.ProjectMembers.FirstOrDefaultAsync(b => b.MemberId == user.Id && b.ProjectId == id);
            
            if (projectMembers != null)
            {
                _context.ProjectMembers.Remove(projectMembers);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("MyProjects", "Account");
        }
        
    }
}
