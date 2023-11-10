using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.JointEntities;

namespace ArgoCMS.Pages.Admin.EmployeeNotificationGroup
{
    public class IndexModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public IndexModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ArgoCMS.Models.JointEntities.EmployeeNotificationGroup> EmployeeNotificationGroup { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.EmployeeNotificationGroups != null)
            {
                EmployeeNotificationGroup = await _context.EmployeeNotificationGroups
                .Include(e => e.Employee)
                .Include(e => e.NotificationGroup).ToListAsync();
            }
        }
    }
}
