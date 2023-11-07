using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.Comments;

namespace ArgoCMS.Pages.Admin.Comments.JobComments
{
    public class IndexModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public IndexModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<JobComment> JobComment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.JobComments != null)
            {
                JobComment = await _context.JobComments
                .Include(j => j.Job)
                .Include(j => j.Owner).ToListAsync();
            }
        }
    }
}
