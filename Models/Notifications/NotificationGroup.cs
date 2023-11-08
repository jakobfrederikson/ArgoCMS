using ArgoCMS.Models.JointEntities;
using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Models.Notifications
{
    public class NotificationGroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public List<EmployeeNotificationGroup> EmployeeNotificationGroups { get; set; }
    }
}
