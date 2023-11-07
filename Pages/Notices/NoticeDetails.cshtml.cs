using ArgoCMS.Data;
using ArgoCMS.Models;
using ArgoCMS.Models.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.Notices
{
    public class NoticeDetailsModel : DependencyInjection_BasePageModel
    {
        public NoticeDetailsModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<Employee> userManager)
            : base(context, authorizationService, userManager)
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

        public async Task<IActionResult> OnPostAsync() 
        {
            if (NoticeComment.CommentText == null)
            {
                return Page();
            }

            NoticeComment.CreationDate = DateTime.Now;
            NoticeComment.OwnerID = UserManager.GetUserId(User);

            Context.NoticeComments.Add(NoticeComment);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the comment. Please try again later.");
                return Page();
            }

            NoticeComment.CommentText = string.Empty;

            return RedirectToPage("NoticeDetails", new { id = NoticeComment.ParentId });
        }
    }
}
