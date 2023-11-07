using ArgoCMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.api.Job
{
    public class GetEmployeesByTeamModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public GetEmployeesByTeamModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int teamId)
        {
            var employees = _context.EmployeeTeams
                .Include(et => et.Employee)
                .Include(et => et.Team)
                .Where(t => t.TeamId == teamId)
                .Select( et =>
                    new SelectListItem
                    {
                        Text = et.Employee.FullName,
                        Value = et.Employee.Id
                    }
                );

            return new JsonResult(employees);
        }
    }
}
