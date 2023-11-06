using ArgoCMS.Models.JointEntities;
using ArgoCMS.Models.Notifications;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Models
{
    public class Employee : IdentityUser
    {
        // user ID of the person they report to
        [Required]
        [Display(Name = "Reports to")]
        public string ReportsToId { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Personal email")]
        public string PersonalEmail { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Employment date")]
        public DateTime EmploymentDate { get; set; }

        [Display(Name = "Full name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public List<Job> Jobs { get; set; }
        public List<EmployeeProject> EmployeeProjects { get; set; }
        public List<EmployeeTeam> EmployeeTeams { get; set; }
        public List<NotificationGroup> NotificationGroups { get; set; }
    }
}
