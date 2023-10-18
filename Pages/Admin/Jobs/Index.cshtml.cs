using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.Admin.Jobs
{
    [Authorize(Roles = "Administrators")]
    public class IndexModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public IndexModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Job> Job { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Jobs != null)
            {
                Job = await _context.Jobs.ToListAsync();
            }
        }
    }
}
