using ArgoCMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.Admin.Employees
{
    public class DeleteModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;
        
        public DeleteModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        public async Task<IActionResult> OnGet(string id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                Employee = employee;
            }
            return Page();
        }

		public async Task<IActionResult> OnPostAsync(string id)
		{
			if (id == null || _context.Employees == null)
			{
				return NotFound();
			}
			var employee = await _context.Employees.FindAsync(id);

			if (employee != null)
			{
				Employee = employee;
				_context.Employees.Remove(Employee);
				await _context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}
