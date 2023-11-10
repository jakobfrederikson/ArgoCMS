using ArgoCMS.Data;
using ArgoCMS.Hubs;
using ArgoCMS.Models;
using ArgoCMS.Models.Comments;
using ArgoCMS.Models.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ArgoCMS.Pages.Notices
{
    public class NoticeDetailsModel : DependencyInjection_Hub_BasePageModel
    {
        public NoticeDetailsModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<Employee> userManager,
            IHubContext<NotificationHub> hubContext)
            : base(context, authorizationService, userManager, hubContext)
        {
        }

        [BindProperty]
        public Notice Notice { get; set; }
        [BindProperty]
        public NoticeComment NoticeComment { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null || Context.Notices == null)
            {
                return NotFound("Notice does not exist.");
            }

            var notice = await Context.Notices
                .Include(n => n.Owner)
                .Include(n => n.Comments)
                .ThenInclude(c => c.Owner)
                .FirstOrDefaultAsync(n => n.NoticeId == id);

            if (notice == null)
            {
                return NotFound();
            }
            else
            {
                Notice = notice;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) 
        {
            if (NoticeComment.CommentText == null)
            {
                return Page();
            }

            var notice = Context.Notices.FirstOrDefault(n => n.NoticeId == id);

            if (notice == null)
            {
                return NotFound();
            }

            NoticeComment.CreationDate = DateTime.Now;
            NoticeComment.OwnerID = UserManager.GetUserId(User);
            NoticeComment.Notice = notice;

            var notificationGroup = DetermineNotificationGroup(NoticeComment.Notice);
            var notification = new NoticeCommentNotification();
            notification.SetNoticeCommentNotification(NoticeComment, notificationGroup.Id);
            
            Context.NoticeComments.Add(NoticeComment);
            Context.NoticeCommentNotifications.Add(notification);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the comment. Please try again later.");
                return Page();
            }

            await HubContext.Clients.Group(notificationGroup.GroupName).SendAsync("ReceiveNoticeNotification",
                JsonConvert.SerializeObject(notification));

            NoticeComment.CommentText = string.Empty;

            return RedirectToPage("NoticeDetails", new { id = NoticeComment.ParentId });
        }

        private NotificationGroup DetermineNotificationGroup(Notice notice)
        {
            if (notice.PublicityStatus == PublicityStatus.Company)
                return Context.NotificationGroups.FirstOrDefault(ng => ng.GroupName == "Company");
            else if (notice.PublicityStatus == PublicityStatus.Project)
            {
                var projectName = Context.Projects
                    .Where(p => p.ProjectId == notice.ProjectId)
                    .Select(p => p.ProjectName)
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(projectName))
                {
                    return Context.NotificationGroups.FirstOrDefault(ng => ng.GroupName == projectName);
                }
            }
            else if (notice.PublicityStatus == PublicityStatus.Team)
            {
                var teamName = Context.Teams
                    .Where(t => t.TeamId == notice.TeamId)
                    .Select(t => t.TeamName)
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(teamName))
                {
                    return Context.NotificationGroups.FirstOrDefault(ng => ng.GroupName == teamName);
                }
            }

            return null;
        }
    }
}
