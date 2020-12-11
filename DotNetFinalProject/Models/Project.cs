using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetFinalProject.Models
{
    public class Project : IValidatableObject
    {
        public Project()
        {
        }

        [Key]
        public long Id { get; set; }

        [Required]
        [DisplayName("Project Name")]
        public string Name { get; set; }
        
        [Required]
        [DisplayName("Project Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Owner Id")]
        public string OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public User Owner { get; set; }

        [DisplayName("List of Members")]
        public IList<ProjectMember> ProjectMembers { get; set; }
        
        [DisplayName("List of Specialties")]
        public IList<Specialty> Specialties { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name.Length > 30)
            {
                yield return new ValidationResult("Project name can't be more then 30 symbols.");
            }
        }

    }
}