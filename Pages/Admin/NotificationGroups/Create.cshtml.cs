using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArgoCMS.Data;
using ArgoCMS.Models.Notifications;

namespace ArgoCMS.Pages.Admin.NotificationGroups
{
    public class CreateModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public CreateModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public NotificationGroup NotificationGroup { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.NotificationGroups == null || NotificationGroup == null)
            {
                return Page();
            }

            _context.NotificationGroups.Add(NotificationGroup);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
