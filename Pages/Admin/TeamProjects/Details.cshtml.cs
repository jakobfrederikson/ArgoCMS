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
    public class DetailsModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public DetailsModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public TeamProject TeamProject { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.TeamProjects == null)
            {
                return NotFound();
            }

            var teamproject = await _context.TeamProjects.FirstOrDefaultAsync(m => m.TeamId == id);
            if (teamproject == null)
            {
                return NotFound();
            }
            else 
            {
                TeamProject = teamproject;
            }
            return Page();
        }
    }
}
