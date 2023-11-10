using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ArgoCMS.Models.Notifications
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
		public bool IsRead { get; set; }
		public DateTime TimeStamp { get; set; }

		public string URL { get; set; }
        public string ObjectId { get; set; }

        public string? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        public Notification()
        {
            // Default values for IsRead and TimeStamp
            IsRead = false;
            TimeStamp = DateTime.Now;
        }
    }
}
