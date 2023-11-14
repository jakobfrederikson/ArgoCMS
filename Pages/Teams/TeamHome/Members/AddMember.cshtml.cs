using ArgoCMS.Data;
using ArgoCMS.Models;
using ArgoCMS.Models.JointEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.Teams.TeamHome.Members
{
    public class AddMemberModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddMemberModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string SelectedEmployeeId { get; set; }
        int Id;

        public List<SelectListItem> EmployeeList { get; set; }

        public async Task OnGet(int teamId)
        {
            Id = teamId;

            var employeesAlreadyInTeam = await _context.EmployeeTeams
                .Where(et => et.TeamId == teamId)
                .Select(et => et.EmployeeId)
                .ToListAsync();

            EmployeeList = await _context.Employees
                .Where(e => !employeesAlreadyInTeam.Contains(e.Id))
                .Select(e => new SelectListItem { Value = e.Id, Text = e.FullName })
                .ToListAsync();
        }

		public async Task<IActionResult> OnPost(int teamId)
		{
            if (SelectedEmployeeId == null)
            {
                return NotFound("Selected employee Id was null");
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == SelectedEmployeeId);

            var team = await _context.Teams
                .Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.TeamId == teamId);

            if (employee != null && team != null)
            {
                team.Members.Add(employee);

                EmployeeTeam employeeTeam = new EmployeeTeam
                {
                    EmployeeId = employee.Id,
                    TeamId = team.TeamId
                };

                _context.EmployeeTeams.Add(employeeTeam);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { teamId });

		}
	}
}
