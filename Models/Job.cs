using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Models
{
    /// <summary>
    /// Represents the tasks assigned to employees.
    /// </summary>
    public class Job
    {
        public int JobId { get; set; }

        [Required]
        [Display(Name = "Created by")]
        public string OwnerID { get; set; }
        public Employee Owner { get; set; }

        [Required]
        [Display(Name = "Assigned to")]
        public string AssignedEmployeeID { get; set; }
        public Employee AssignedEmployee { get; set; }

        [Required]
        [Display(Name = "Team")]
        public int TeamID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string JobName { get; set; }

        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string JobDescription { get; set; }

        [Required]
        [Display(Name = "Date created")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        [Required]
        [Display(Name = "Due date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Display(Name = "Job status")]
        public JobStatus JobStatus { get; set; }

        [Display(Name = "Priority level")]
        public PriorityLevel PriorityLevel { get; set; }       
        
        public List<JobComment> Comments { get; set; } // Navigation property
    }

    public enum JobStatus
    {
        Unread,
        Read,
        Working,
        Submitted,
        Completed
    }

    public enum PriorityLevel
    {
        Low,
        Normal,
        Medium,
        High,
        Critical
    }
}
