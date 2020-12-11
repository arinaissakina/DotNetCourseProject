using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetFinalProject.Data;
using DotNetFinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetFinalProject.Services
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly CourseProjectContext _context;

        public ProjectRepository(CourseProjectContext context)
        {
            _context = context;
        }

        public Task<List<Project>> GetAll()
        {
            return _context.Projects.Include(p => p.Owner).ToListAsync();
        }

        public Task<Project> GetOne(long? id)
        {
            return _context.Projects
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public void AddProject(Project project)
        {
            _context.Projects.Add(project);
        }

        public void EditProject(Project project)
        {
            _context.Projects.Update(project);
        }

        public Task Save()
        {
            return _context.SaveChangesAsync();
        }

        public void RemoveProject(Project project)
        {
            _context.Projects.Remove(project);
        }

        public bool Exists(long? id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}