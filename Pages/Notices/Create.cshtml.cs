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

        public IActionResult OnGet()
        {
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

            var notification = new NoticeNotification();
            notification.Message = $"You have a new notice: {Notice.NoticeTitle}";
            notification.URL = "/Notices/NoticeDetails";
            notification.ObjectId = Notice.NoticeId.ToString();
            notification.CompanyWide = Notice.PublicityStatus == PublicityStatus.Company;            

            Context.NoticeNotifications.Add(notification);

            string groupName = DetermineGroupNameForNotification(notification);
            await HubContext.Clients.Group(groupName).SendAsync("ReceiveNoticeNotification", 
                JsonConvert.SerializeObject(notification));
            
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private string DetermineGroupNameForNotification(
            NoticeNotification noticeNotification)
        {
            if (noticeNotification.CompanyWide) return "Company";

            if (Notice.TeamId != null)
            {
                string teamName = Context.Teams
                    .Find(Notice.TeamId)
                    .TeamName;

                return teamName;
            }

            if (Notice.ProjectId != null)
            {
                string projectName = Context.Projects
                    .Find(Notice.ProjectId)
                    .ProjectName;

                return projectName;
            }

            return "";
        }
    }
}
