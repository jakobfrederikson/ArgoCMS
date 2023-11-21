using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Models;
using ArgoCMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ArgoCMS.Models.Comments;
using Microsoft.AspNetCore.SignalR;
using ArgoCMS.Hubs;
using Newtonsoft.Json;
using ArgoCMS.Models.Notifications;

namespace ArgoCMS.Pages.Jobs
{
    public class DetailsModel : DependencyInjection_Hub_BasePageModel
    {
        public DetailsModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<Employee> userManager,
            IHubContext<NotificationHub> hubContext)
            : base (context, authorizationService, userManager, hubContext)
        {
        }

        [BindProperty]
        public Job Job { get; set; } = default!;
        [BindProperty]
        public JobComment JobComment { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || Context.Jobs == null)
            {
                return NotFound();
            }

            var job = await Context.Jobs
                .Include(j => j.Owner)
                .Include(j => j.AssignedEmployee)
                .Include(j => j.Comments)
                .ThenInclude(c => c.Owner)
                .FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null)
            {
                return NotFound();
            }
            else 
            {
                Job = job;
            }

            var userId = UserManager.GetUserId(User);
            if (userId != null)
            {
                if (Job.JobStatus == JobStatus.Unread)
                {
                    if (Job.AssignedEmployeeID == userId)
                    {
                        Job.JobStatus = JobStatus.Read;
                        Context.Jobs.Update(Job);
                        Context.SaveChanges();
                    }       
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (JobComment.CommentText == null)
            {
                return Page();
            }

            var job = Context.Jobs.FirstOrDefault(j => j.JobId == id);

            if (job == null)
            {
                return NotFound();
            }

            JobComment.CreationDate = DateTime.Now;
            JobComment.OwnerID = UserManager.GetUserId(User);
            JobComment.Job = job;

            string receiverId = JobComment.OwnerID == job.OwnerID
                ? job.AssignedEmployeeID
                : job.OwnerID;

            var notification = new JobCommentNotification();
            notification.SetJobCommentNotification(JobComment, receiverId);

            Context.JobCommentsNotifications.Add(notification);
            Context.JobComments.Add(JobComment);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the comment. Please try again later.");
                return Page();
            }

            await HubContext.Clients.User(receiverId)
                .SendAsync("ReceiveJobNotification", JsonConvert.SerializeObject(notification));

            JobComment.CommentText = string.Empty;

            return RedirectToPage("Index");
        }
    }
}
