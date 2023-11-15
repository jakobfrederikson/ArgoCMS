using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArgoCMS.Models;
using ArgoCMS.Models.Notifications;
using ArgoCMS.Data;
using ArgoCMS.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.Notices
{
    public class CreateModel : DependencyInjection_Hub_BasePageModel
    {
        public CreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<Employee> userManager,
            IHubContext<NotificationHub> hubContext)
            : base(context, authorizationService, userManager, hubContext)
        {
        }

        public IActionResult OnGet(int? teamId, int? projectId)
        {
            Notice = new Notice();

            if (teamId != null)
            {
                var team = Context.Teams.FirstOrDefault(t => t.TeamId == teamId);
                if (team != null)
                {
                    Notice.TeamId = team.TeamId;
                    Notice.PublicityStatus = PublicityStatus.Team;
                }                
            }

            if (projectId != null)
            {
                var project = Context.Projects.FirstOrDefault(p => p.ProjectId == projectId);
                if (project != null)
                {
                    Notice.ProjectId = project.ProjectId;
                    Notice.PublicityStatus = PublicityStatus.Project;
                }
            }

            TeamItems = new SelectList(Context.Teams, "TeamId", "TeamName");
            ProjectItems = new SelectList(Context.Projects, "ProjectId", "ProjectName");

            return Page();
        }

        [BindProperty]
        public Notice Notice { get; set; }
        public SelectList TeamItems { get; set; }
        public SelectList ProjectItems { get; set; }

        public NoticeNotification NoticeNotification { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await UserManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Current user doesn't exist");
            }

            Notice.OwnerID = user.Id;
            Notice.DateCreated = DateTime.Now; 

            Context.Notices.Add(Notice);
            await Context.SaveChangesAsync();

            var notificationGroup = DetermineNotificationGroup(Notice);
            var notification = new NoticeNotification();
            notification.SetNoticeNotification(Notice, notificationGroup.Id);

            Context.NoticeNotifications.Add(notification);
            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            await HubContext.Clients.Group(notificationGroup.GroupName).SendAsync("ReceiveNoticeNotification", 
                JsonConvert.SerializeObject(notification, jss));
            
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
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
