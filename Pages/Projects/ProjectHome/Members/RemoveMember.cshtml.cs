using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.Projects.ProjectHome.Members
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

        public int projectId { get; set; }

        public async Task<IActionResult> OnGet(string memberId, int projectId)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == memberId);

            if (employee == null)
            {
                return NotFound("The employee with the specified ID was not found.");
            }

            Employee = employee;

            this.projectId = projectId;

            return Page();
        }

        public async Task<IActionResult> OnPost(string memberId, int projectId)
        {
            if (Employee == null)
            {
                return NotFound("Employee not found.");
            }

            var employeeProject = await _context.EmployeesProjects
                .FirstOrDefaultAsync(ep => ep.EmployeeId == memberId && ep.ProjectId == projectId);

            if (employeeProject != null)
            {
                _context.EmployeesProjects.Remove(employeeProject);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index", new { projectId });
            }

            return NotFound("Employee is not a member of the specified project.");
        }
    }
}
