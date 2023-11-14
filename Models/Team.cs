using ArgoCMS.Models.JointEntities;
using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Models
{
    public class Team
    {
        public int TeamId { get; set; }

        public string? TeamLeaderId { get; set; } // Team Leader (optional)
        public Employee TeamLeader { get; set; } // Navigation property for the team leader

        public string? CreatedById { get; set; }
        public Employee CreatedBy { get; set; } // Navigation property for the creator

		[Required]
        [Display(Name = "Name")]
        public string TeamName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string TeamDescription { get; set; }

        [Required]
        [Display(Name = "Date created")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        public List<Job> Jobs { get; set; }
        public List<Employee> Members { get; set; }
        public List<EmployeeTeam> EmployeeTeams { get; set; } // Collection of team members
        public List<TeamProject> TeamProjects { get; set; }
    }
}
