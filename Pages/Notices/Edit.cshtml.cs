using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgoCMS.Pages.Notices
{
    public class EditModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public EditModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public class NoticeViewModel
        {
            public int NoticeId { get; set; }
            public string OwnerID { get; set; }
            public string NoticeTitle { get; set; }
            public string NoticeMessageContent { get; set; }
            public PublicityStatus PublicityStatus { get; set; }
            public DateTime DateCreated { get; set; }
            public int? TeamId { get; set; }
            public int? ProjectId { get; set; }
        }

        [BindProperty]
        public NoticeViewModel Notice { get; set; } = default!;

        public SelectList TeamItems { get; set; }
        public SelectList ProjectItems { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Notices == null)
            {
                return NotFound();
            }

            var notice =  await _context.Notices.FirstOrDefaultAsync(m => m.NoticeId == id);

            if (notice == null)
            {
                return NotFound();
            }

            Notice = new NoticeViewModel
            {
                NoticeId = notice.NoticeId,
                OwnerID = notice.OwnerID,
                NoticeTitle = notice.NoticeTitle,
                NoticeMessageContent = notice.NoticeMessageContent,
                DateCreated = notice.DateCreated,
                PublicityStatus = notice.PublicityStatus,
                TeamId = notice.TeamId,
                ProjectId = notice.ProjectId
            };

            TeamItems = new SelectList(_context.Teams, "TeamId", "TeamName");
            ProjectItems = new SelectList(_context.Projects, "ProjectId", "ProjectName");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var notice = await _context.Notices.
                FirstOrDefaultAsync(n => n.NoticeId == Notice.NoticeId);

            if (notice == null)
            {
                return NotFound("The notice could not be found.");
            }

            notice.NoticeTitle = Notice.NoticeTitle;
            notice.NoticeMessageContent = Notice.NoticeMessageContent;
            notice.DateCreated = Notice.DateCreated;
            notice.PublicityStatus = Notice.PublicityStatus;
            notice.TeamId = Notice.TeamId;
            notice.ProjectId = Notice.ProjectId;

            _context.Attach(notice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoticeExists(notice.NoticeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool NoticeExists(int id)
        {
          return _context.Notices.Any(e => e.NoticeId == id);
        }
    }
}
