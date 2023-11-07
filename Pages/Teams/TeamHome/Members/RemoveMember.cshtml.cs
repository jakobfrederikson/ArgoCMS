using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.Teams.TeamHome.Members
{
    public class RemoveMemberModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public RemoveMemberModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        public int teamId { get; set; }

        public async Task<IActionResult> OnGet(string memberId, int teamId)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == memberId);

            if (employee == null)
            {
                return NotFound("The employee with the specified ID was not found.");
            }

            Employee = employee;

            this.teamId = teamId;

            return Page();
        }

        public async Task<IActionResult> OnPost(string memberId, int teamId)
        {
            if (Employee == null)
            {
                return NotFound("Employee not found.");
            }

            var employeeTeam = await _context.EmployeeTeams
                .FirstOrDefaultAsync(et => et.EmployeeId == memberId && et.TeamId == teamId);

            if (employeeTeam != null)
            {
                _context.EmployeeTeams.Remove(employeeTeam);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index", new { teamId });
            }

            return NotFound("Employee is not a member of the specified team.");
        }
    }
}
