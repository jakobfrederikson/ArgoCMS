namespace ArgoCMS.Models.JointEntities
{
	public class EmployeeProject
	{
		public string EmployeeId { get; set; }
		public Employee Employee { get; set; }

		public int ProjectId { get; set; }
		public Project Project { get; set; }

		public bool IsCompleted { get; set; }
	}
}
