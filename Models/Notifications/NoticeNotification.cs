using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Security.Policy;

namespace ArgoCMS.Models.Notifications
{
    public class NoticeNotification : Notification
    {
        public int? TeamId { get; set; }
        public int? ProjectId { get; set; }

        public bool CompanyWide { get; set; }

        public int NotificationGroupId { get; set; }
        public NotificationGroup NotificationGroup { get; set; }

        public void SetNoticeNotification(Notice notice, int notificationGroupId)
        {
            Message = $"You have a new notice: {notice.NoticeTitle}";
            URL = "/Notices/NoticeDetails";
            ObjectId = notice.NoticeId.ToString();

            this.NotificationGroupId = notificationGroupId;

            CompanyWide = notice.PublicityStatus == PublicityStatus.Company;

            if (!CompanyWide)
            {
                if (notice.TeamId != null)
                {
                    this.TeamId = notice.TeamId;
                }
                else if (notice.ProjectId != null)
                {
                    this.ProjectId = notice.ProjectId;
                }
            }
        }
    }
}
