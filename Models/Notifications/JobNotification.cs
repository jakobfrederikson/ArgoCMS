namespace ArgoCMS.Models.Notifications
{
    public class JobNotification : Notification
    {
        public JobNotification(Job job)
        {
            Message = $"New job assigned: {job.JobName}";
            URL = "/Jobs/Details";
            ObjectId = job.JobId.ToString();
            UserId = job.AssignedEmployeeID;
        }
    }
}
