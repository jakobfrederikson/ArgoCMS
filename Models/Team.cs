using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Models
{
    public class Team
    {
        public int TeamId { get; set; }

        [Display(Name = "Created by")]
        public string OwnerID { get; set; }

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

        public ICollection<Employee> Employees { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
