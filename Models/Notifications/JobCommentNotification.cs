using ArgoCMS.Models.Comments;

namespace ArgoCMS.Models.Notifications
{
    public class JobCommentNotification : Notification
    {
        public void SetJobCommentNotification(JobComment jobComment, string receiverId)
        {
            Message = $"New comment on job: {jobComment.Job.JobName}";
            URL = "/Jobs/Details";
            ObjectId = jobComment.Job.JobId.ToString();
            EmployeeId = receiverId;
        }
    }
}
