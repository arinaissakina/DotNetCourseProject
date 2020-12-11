using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DotNetFinalProject.Models
{
    public class Specialty
    {
        public Specialty()
        {
        }
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Specialty name can't be more than 15 symbols.")]
        [DisplayName("Specialty Name")]
        
        public string Name { get; set; }

        [DisplayName("Users")]
        public IList<SpecialtyUser> SpecialityUsers { get; set; }

    }
}