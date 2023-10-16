using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArgoCMS.Areas.Identity.Pages.Account.Manage
{
    public class DbContextPageModel : PageModel
    {
        protected ApplicationDbContext Context { get; }

        public DbContextPageModel(
            ApplicationDbContext context) : base()
        {
            Context = context;
        }
    }
}
