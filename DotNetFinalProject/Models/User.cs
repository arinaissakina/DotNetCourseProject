using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;


namespace DotNetFinalProject.Models
{
    public class User : IdentityUser
    {
        
        [Required]
        [NotContainsDigits]
        [DisplayName("Full Name")]
        public string Name { get; set; }

        [DisplayName("Specialties")]
        public IList<SpecialtyUser> Specialties { get; set;}

        [DisplayName("Project Member")]
        public IList<ProjectMember> ProjectMember { get; set;}
        
        [DisplayName("Project Owner")]
        public IList<Project> ProjectOwner { get; set;}
    }
    
    public class NotContainsDigitsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                String stringValue = value.ToString();
                if (stringValue.Any(char.IsDigit) == false)
                    return ValidationResult.Success;     
            }

            return new ValidationResult("Name can't contain digits.");
        }
    }
}