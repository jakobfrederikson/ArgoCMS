using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.Comments;

namespace ArgoCMS.Pages.Admin.Comments.NoticeComments
{
    public class IndexModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public IndexModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<NoticeComment> NoticeComment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.NoticeComments != null)
            {
                NoticeComment = await _context.NoticeComments
                .Include(n => n.Notice)
                .Include(n => n.Owner).ToListAsync();
            }
        }
    }
}
