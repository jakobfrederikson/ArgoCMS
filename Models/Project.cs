using ArgoCMS.Models.JointEntities;
using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required]
        [Display(Name = "Created by")]
        public string OwnerID { get; set; }
        public Employee Owner { get; set; }

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

		[Required]
		[Display(Name = "Due Date")]
		[DataType(DataType.Date)]
		public DateTime DueDate { get; set; }

		[Required]
        [Display(Name = "Project status")]
        public ProjectStatus ProjectStatus { get; set; }

        public List<Employee> Members { get; set; }
        public ICollection<EmployeeProject> EmployeeProjects { get; set; }
        public ICollection<TeamProject> TeamProjects { get; set; }
    }

    public enum ProjectStatus
    {
        NotStarted,
        InProgress,
        Completed
    }
}
