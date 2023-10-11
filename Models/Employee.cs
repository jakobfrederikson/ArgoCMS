using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Models
{
    /// <summary>
    /// The identity model that all users derive from.
    /// </summary>
    public class Employee : IdentityUser
    {
        // user ID of the person they report to
        [Required]
        [Display(Name = "Reports to")]        
        public string ReportsToId { get; set; }

        [Required]
        [Display(Name = "Team ID")]
        public int TeamID { get; set; }

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

        public string FullName { get
            {
                return FirstName + " " + LastName;
            }
        }

        public ICollection<Job> Jobs { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
