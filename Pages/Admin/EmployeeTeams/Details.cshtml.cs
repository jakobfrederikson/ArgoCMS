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
    public class DetailsModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public DetailsModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public EmployeeTeam EmployeeTeam { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.EmployeeTeams == null)
            {
                return NotFound();
            }

            var employeeteam = await _context.EmployeeTeams.FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employeeteam == null)
            {
                return NotFound();
            }
            else 
            {
                EmployeeTeam = employeeteam;
            }
            return Page();
        }
    }
}
