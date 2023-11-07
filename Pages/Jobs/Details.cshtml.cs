using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Models;
using ArgoCMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ArgoCMS.Models.Comments;

namespace ArgoCMS.Pages.Jobs
{
    public class DetailsModel : DependencyInjection_BasePageModel
    {
        public DetailsModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<Employee> userManager)
            : base (context, authorizationService, userManager)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (JobComment.CommentText == null)
            {
                return Page();
            }

            JobComment.CreationDate = DateTime.Now;
            JobComment.OwnerID = UserManager.GetUserId(User);

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

            JobComment.CommentText = string.Empty;

            return RedirectToPage("Details", new { id = JobComment.ParentId });
        }
    }
}
