using System.Collections.Generic;
using DotNetFinalProject.Models;

namespace DotNetFinalProject.ViewModels
{
    public class EditSpecialtyViewModel
    {
        public User User { get; set; }  
        public IEnumerable<Specialty> Specialties { get; set; }
        public IEnumerable<SpecialtyUser> SpecialtyUsers { get; set; }
    }
}