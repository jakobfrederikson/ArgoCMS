using ArgoCMS.Models.Notifications;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArgoCMS.Models.JointEntities
{
	public class EmployeeNotificationGroup
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string EmployeeId { get; set; }
		public Employee Employee { get; set; }

		public int NotificationGroupId { get; set; }
		public NotificationGroup NotificationGroup { get; set; }
	}
}
