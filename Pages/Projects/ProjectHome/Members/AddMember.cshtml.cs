using ArgoCMS.Data;
using ArgoCMS.Models.JointEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.Projects.ProjectHome.Members
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

        public async Task OnGet(int projectId)
        {
            Id = projectId;

            var employeesAlreadyInProject = await _context.EmployeesProjects
                .Where(et => et.ProjectId == projectId)
                .Select(et => et.EmployeeId)
                .ToListAsync();

            EmployeeList = await _context.EmployeesProjects
                .Where(ep => !employeesAlreadyInProject.Contains(ep.EmployeeId))
                .Select(ep => new SelectListItem { Value = ep.EmployeeId, Text = ep.Employee.FullName })
                .Distinct()
                .ToListAsync();
        }

        public async Task<IActionResult> OnPost(int projectId)
        {
            if (SelectedEmployeeId == null)
            {
                return NotFound("Selected employee Id was null");
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == SelectedEmployeeId);

            var project = await _context.Projects
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            if (employee != null && project != null)
            {
                project.Members.Add(employee);

                EmployeeProject employeeProject = new EmployeeProject
                {
                    EmployeeId = employee.Id,
                    ProjectId = project.ProjectId
                };

                _context.EmployeesProjects.Add(employeeProject);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { projectId });

        }
    }
}
