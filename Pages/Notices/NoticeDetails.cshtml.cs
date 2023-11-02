using ArgoCMS.Data;
using ArgoCMS.Models;
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

        public Notice Notice { get; set; }

        public NoticeComment Comment { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound($"Notice does not exist.");
            }

            var notice = await Context.Notices
                .Include(n => n.Comments)
                .FirstOrDefaultAsync(n => n.NoticeId == id);

            if (notice != null)
            {
                Notice = notice;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id) 
        {

            return Page();
        }
    }
}
