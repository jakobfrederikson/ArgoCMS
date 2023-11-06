using ArgoCMS.Models.Comments;

namespace ArgoCMS.Models.Notifications
{
    public class JobCommentNotification : Notification
    {
        public JobCommentNotification(JobComment jobComment)
        {
            Message = $"New comment on job: {jobComment.Job.JobName}";
            URL = "/Jobs/Details";
            ObjectId = jobComment.Job.JobId.ToString();
            UserId = jobComment.Job.AssignedEmployeeID;
        }
    }
}
