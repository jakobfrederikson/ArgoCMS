using ArgoCMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Services.Jobs
{
    public class JobService : IJobService
    {
        private readonly ApplicationDbContext _context;

        public JobService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IQueryable<SelectListItem> GetEmployeesByTeam(int teamId)
        {
            var employees = _context.EmployeeTeams
                .Include(et => et.Employee)
                .Include(et => et.Team)
                .Where(t => t.TeamId == teamId)
                .Select(et =>
                    new SelectListItem
                    {
                        Text = et.Employee.FullName,
                        Value = et.Employee.Id
                    }
                );

            return employees;
        }
    }
}
