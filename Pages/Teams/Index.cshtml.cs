using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ArgoCMS.Pages.Teams
{
    public class IndexModel : DependencyInjection_BasePageModel
    {
        public IndexModel(
        ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<Employee> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        public Team Team { get; set; } = default!;
        public IList<Job> TeamJobs { get; set;} = default!;
        public IList<Notice> TeamNotices { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var userId = UserManager.GetUserId(User);

            var user = await Context.Employees.FirstOrDefaultAsync(e => e.Id == userId);

            var team = await Context.Teams
                .Include(t => t.Employees.Where(e => e.TeamID == user.TeamID))
                .FirstOrDefaultAsync(t => t.TeamId == user.TeamID);

            if (team != null)
            {
                Team = team;
            }

            var teamJobs = Context.Jobs
                .Where(j => j.TeamID == Team.TeamId).ToList();

            if (teamJobs != null)
            {
                TeamJobs = teamJobs;
            }

            var teamNotices = Context.Notices
                .Where(n => n.TeamId == Team.TeamId).ToList();

            if (teamNotices != null)
            {
                TeamNotices = teamNotices;
            }
        }
    }
}
