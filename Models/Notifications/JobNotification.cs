namespace ArgoCMS.Models.Notifications
{
    public class JobNotification : Notification
    {
        public void SetJobNotification(Job job)
        {
            Message = $"New job assigned: {job.JobName}";
            URL = "/Jobs/Details";
            ObjectId = job.JobId.ToString();
            EmployeeId = job.AssignedEmployeeID;
        }
    }
}
