using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace DotNetFinalProject.Models
{
    public class ProjectMember
    {
        public ProjectMember()
        {
        }
        
        [Required]
        [DisplayName("Project Id")]
        public long ProjectId { get; set; }
        
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [Required]
        [DisplayName("Member Id")]
        public string MemberId { get; set; }
        
        [ForeignKey("MemberId")]
        public User Member { get; set; }
    }
}