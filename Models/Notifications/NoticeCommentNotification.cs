using ArgoCMS.Models.Comments;

namespace ArgoCMS.Models.Notifications
{
    public class NoticeCommentNotification : Notification
    {
        public int? TeamId { get; set; }
        public int? ProjectId { get; set; }

        public bool CompanyWide { get; set; }

        public int NotificationGroupId { get; set; }
        public NotificationGroup NotificationGroup { get; set; }

        public void SetNoticeCommentNotification(NoticeComment noticeComment, int notificationGroupId)
        {
            Message = $"New comment on notice: {noticeComment.Notice.NoticeTitle}";
            URL = "/Notices/NoticeDetails";
            ObjectId = noticeComment.Notice.NoticeId.ToString();

            this.NotificationGroupId = notificationGroupId;

            CompanyWide = noticeComment.Notice.PublicityStatus == PublicityStatus.Company;

            if (!CompanyWide)
            {
                if (noticeComment.Notice.TeamId != null)
                {
                    this.TeamId = noticeComment.Notice.TeamId;
                }
                else if (noticeComment.Notice.ProjectId != null)
                {
                    this.ProjectId = noticeComment.Notice.ProjectId;
                }
            }
        }
    }
}
