using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.Notifications;

namespace ArgoCMS.Pages.Admin.NotificationGroups
{
    public class IndexModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public IndexModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<NotificationGroup> NotificationGroup { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.NotificationGroups != null)
            {
                NotificationGroup = await _context.NotificationGroups.ToListAsync();
            }
        }
    }
}
