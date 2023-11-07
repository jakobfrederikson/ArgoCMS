using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArgoCMS.Models.JointEntities
{
	public class EmployeeProject
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

		public string EmployeeId { get; set; }
		public Employee Employee { get; set; }

		public int ProjectId { get; set; }
		public Project Project { get; set; }

		public bool IsCompleted { get; set; }
	}
}
