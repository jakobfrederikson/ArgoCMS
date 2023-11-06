namespace ArgoCMS.Models.Notifications
{
    public class NoticeNotification : Notification
    {
        public NoticeNotification()
        {
            //Message = $"You have a new notice: {notice.NoticeTitle}";
            //URL = "/Notices/NoticeDetails";
            //ObjectId = notice.NoticeId.ToString();

            //if (!CompanyWide)
            //{
            //    if (notice.TeamId != null)
            //    {
            //        this.TeamId = notice.TeamId;
            //    }
            //    else if (notice.ProjectId != null)
            //    {
            //        this.ProjectId = notice.ProjectId;
            //    }
            //}
        }

        public int? TeamId { get; set; }
        public int? ProjectId { get; set; }

        public bool CompanyWide { get; set; }
    }
}
