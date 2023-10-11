using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models;

namespace ArgoCMS.Pages.Notices
{
    public class IndexModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public IndexModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Notice> Notice { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Notices != null)
            {
                Notice = await _context.Notices.ToListAsync();
            }
        }
    }
}
