﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ArgoCMS.Hubs;
using ArgoCMS.Models.Notifications;
using Newtonsoft.Json;

namespace ArgoCMS.Pages.Jobs
{
    public class CreateModel : DependencyInjection_Hub_BasePageModel
    {
        public CreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<Employee> userManager,
            IHubContext<NotificationHub> hubContext)
            : base(context, authorizationService, userManager, hubContext)
        {
        }

        public IActionResult OnGet(int? teamId, int? projectId)
        {
            Job = new Job();

            if (teamId != null)
            {
                var team = Context.Teams.FirstOrDefault(t => t.TeamId == teamId);
                if (team != null) Job.TeamID = team.TeamId;
            }

            if (projectId != null)
            {
                var project = Context.Projects.FirstOrDefault(p => p.ProjectId == projectId);
                if (project != null) Job.ProjectID = project.ProjectId;
            }

            Job.JobStatus = JobStatus.Unread;
            Job.DueDate = DateTime.Now;
            Employees = Context.Employees.Select(
                    e => new SelectListItem { Text = e.FullName, Value = e.Id });

            Teams = Context.Teams.Select(
                t => new SelectListItem { Text = t.TeamName, Value = t.TeamId.ToString() });

            Projects = Context.Projects.Select(
                p => new SelectListItem { Text = p.ProjectName, Value = p.ProjectId.ToString() });

            var currId = UserManager.GetUserId(User);
            var currUser = Context.Employees.FirstOrDefault(e => e.Id == currId);

            if (currUser == null)
            {
                return Forbid($"Could not find a user with the id: {currId}");
            }

            return Page();
        }

        [BindProperty]
        public Job Job { get; set; }

        public string OwnerID { get; set; }
        public IEnumerable<SelectListItem> Employees { get; set; }
        public IEnumerable<SelectListItem> Teams { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (Job.TeamID == null
             || Job.AssignedEmployeeID == null
             || Job.JobName == null
             || Job.JobDescription == null)
            {
                return Page();
            }

            Job.OwnerID = UserManager.GetUserId(User);
            Job.DateCreated = DateTime.Now;
            Job.JobStatus = JobStatus.Unread;

            Context.Jobs.Add(Job);
            await Context.SaveChangesAsync();

            var notification = new JobNotification();
            notification.SetJobNotification(Job);

            Context.Notifications.Add(notification);
            await Context.SaveChangesAsync();

            await HubContext.Clients.User(notification.EmployeeId)
                .SendAsync("ReceiveJobNotification", JsonConvert.SerializeObject(notification));

            return RedirectToPage("./Index");
        }
    }
}
