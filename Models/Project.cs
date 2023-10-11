using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required]
        [Display(Name = "Created by")]
        public string OwnerID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string ProjectName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string ProjectDescription { get; set; }

        [Required]
        [Display(Name = "Date created")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        public ICollection<Employee> Employees { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}
