using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetFinalProject.Data;
using DotNetFinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetFinalProject.Services
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        
        private readonly CourseProjectContext _context;

        public SpecialtyRepository(CourseProjectContext context)
        {
            _context = context;
        }
        
        public Task<List<Specialty>> GetAll()
        {
            return _context.Specialties.ToListAsync();
        }

        public Task<Specialty> GetOne(long? id)
        {
            return _context.Specialties
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public void AddSpecialty(Specialty specialty)
        {
            _context.Specialties.Add(specialty);
        }

        public void EditSpecialty(Specialty specialty)
        {
            _context.Specialties.Update(specialty);
        }

        public Task Save()
        {
            return _context.SaveChangesAsync();
        }

        public void RemoveSpecialty(Specialty specialty)
        {
            _context.Specialties.Remove(specialty);
        }

        public bool Exists(long? id)
        {
            return _context.Specialties.Any(e => e.Id == id);
        }
        
    }
}