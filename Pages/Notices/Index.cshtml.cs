using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Models;
using ArgoCMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ArgoCMS.Pages.Notices
{
    public class IndexModel : DependencyInjection_BasePageModel
    {
        public IndexModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<Employee> userManager)
            : base(context, authorizationService, userManager) 
        { 
        }

        public IList<Notice> Notices { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (Context.Notices != null)
            {
                var userId = UserManager.GetUserId(User);
                Notices = await Context.Notices
                    .Where(n => n.OwnerID == userId)
                    .Include(n => n.Team)
                    .Include(n => n.Project)
                    .ToListAsync();
            }
        }
    }
}
