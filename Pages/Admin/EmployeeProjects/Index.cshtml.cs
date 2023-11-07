using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.JointEntities;

namespace ArgoCMS.Pages.Admin.EmployeeProjects
{
    public class IndexModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public IndexModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<EmployeeProject> EmployeeProject { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.EmployeesProjects != null)
            {
                EmployeeProject = await _context.EmployeesProjects
                .Include(e => e.Employee)
                .Include(e => e.Project).ToListAsync();
            }
        }
    }
}
