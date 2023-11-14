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

        public Employee Employee { get; set; }

        public int teamId { get; set; }

        public async Task<IActionResult> OnGetAsync(string memberId, int teamId)
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

        public async Task<IActionResult> OnPostAsync(string memberId, int teamId)
        {
            var employee = _context.Employees
                .FirstOrDefault(e => e.Id == memberId);

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            Employee = employee;

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
