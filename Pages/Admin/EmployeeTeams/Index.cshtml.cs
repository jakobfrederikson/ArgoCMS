using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.JointEntities;

namespace ArgoCMS.Pages.Admin.EmployeeTeams
{
    public class IndexModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public IndexModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<EmployeeTeam> EmployeeTeam { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.EmployeeTeams != null)
            {
                EmployeeTeam = await _context.EmployeeTeams
                .Include(e => e.Employee)
                .Include(e => e.Team).ToListAsync();
            }
        }
    }
}
