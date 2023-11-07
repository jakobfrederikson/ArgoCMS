using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.JointEntities;

namespace ArgoCMS.Pages.Admin.TeamProjects
{
    public class IndexModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public IndexModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<TeamProject> TeamProject { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.TeamProjects != null)
            {
                TeamProject = await _context.TeamProjects
                .Include(t => t.Project)
                .Include(t => t.Team).ToListAsync();
            }
        }
    }
}
