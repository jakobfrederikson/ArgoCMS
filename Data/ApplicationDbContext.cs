using ArgoCMS.Models;
using ArgoCMS.Models.Comments;
using ArgoCMS.Models.JointEntities;
using ArgoCMS.Models.Notifications;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ArgoCMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
		public ApplicationDbContext()
		{
		}

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
            // Jobs
            builder.Entity<Job>()
                .HasOne(j => j.Owner)
                .WithMany()
                .HasForeignKey(j => j.OwnerID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Job>()
                .HasOne(j => j.AssignedEmployee)
                .WithMany()
                .HasForeignKey(j => j.AssignedEmployeeID);

            builder.Entity<Job>()
                .HasMany(j => j.Comments)
                .WithOne(jc => jc.Job)
                .HasForeignKey(jc => jc.ParentId);

            builder.Entity<Job>()
                .HasOne(j => j.Team)
                .WithMany(t => t.Jobs)
                .HasForeignKey(j => j.TeamID)
                .IsRequired(false);

            builder.Entity<Job>()
                .HasOne(j => j.Project)
                .WithMany(p => p.Jobs)
                .HasForeignKey(j => j.ProjectID)
                .IsRequired(false);

            // Employees
            builder.Entity<Employee>()
                .HasMany(e => e.Jobs)
                .WithOne(j => j.AssignedEmployee)
                .HasForeignKey(e => e.AssignedEmployeeID);

            // Teams
            builder.Entity<Team>()
                .HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Team>()
                .HasOne(t => t.TeamLeader)
                .WithMany()
                .HasForeignKey(t => t.TeamLeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Team>()
                .HasMany(t => t.Jobs)
                .WithOne(j => j.Team)
                .HasForeignKey(j => j.TeamID);

            // Projects
            builder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany()
                .HasForeignKey(p => p.OwnerID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Project>()
                .HasMany(p => p.Jobs)
                .WithOne(j => j.Project)
                .HasForeignKey(j => j.ProjectID);

            // Notices
            builder.Entity<Notice>()
                .HasOne(n => n.Owner)
                .WithMany()
                .HasForeignKey(n => n.OwnerID);

            builder.Entity<Notice>()
                .HasOne(n => n.Team)
                .WithMany()
                .HasForeignKey(n => n.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Notice>()
                .HasOne(n => n.Project)
                .WithMany()
                .HasForeignKey(n => n.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Notice>()
                .HasMany(n => n.Comments)
                .WithOne(nc => nc.Notice)
                .HasForeignKey(nc => nc.ParentId);

            // Join Entities

            // TeamProject
            builder.Entity<TeamProject>()
                .HasKey(tp => tp.Id);

            builder.Entity<TeamProject>()
                .HasKey(tp => new { tp.TeamId, tp.ProjectId });

            builder.Entity<TeamProject>()
                .HasOne(tp => tp.Team)
                .WithMany(t => t.TeamProjects)
                .HasForeignKey(tp => tp.TeamId);

            builder.Entity<TeamProject>()
                .HasOne(tp => tp.Project)
                .WithMany(p => p.TeamProjects)
                .HasForeignKey(tp => tp.ProjectId);

            // EmployeeProject
            builder.Entity<EmployeeProject>()
                .HasKey(ep => ep.Id);

			builder.Entity<EmployeeProject>()
			.HasKey(ep => new { ep.EmployeeId, ep.ProjectId });

			builder.Entity<EmployeeProject>()
				.HasOne(ep => ep.Employee)
				.WithMany(e => e.EmployeeProjects)
				.HasForeignKey(ep => ep.EmployeeId);

			builder.Entity<EmployeeProject>()
				.HasOne(ep => ep.Project)
				.WithMany(p => p.EmployeeProjects)
				.HasForeignKey(ep => ep.ProjectId);

            // EmployeeTeam
            builder.Entity<EmployeeTeam>()
                .HasKey(et => new {et.EmployeeId, et.TeamId });

            builder.Entity<EmployeeTeam>()
                .HasOne(et => et.Employee)
                .WithMany(e => e.EmployeeTeams)
                .HasForeignKey(et => et.EmployeeId);

            builder.Entity<EmployeeTeam>()
                .HasOne(et => et.Team)
                .WithMany(t => t.EmployeeTeams)
                .HasForeignKey(et => et.TeamId);

            // EmployeeNotificationGroup
			builder.Entity<EmployeeNotificationGroup>()
				.HasKey(eng => new { eng.EmployeeId, eng.NotificationGroupId });

			builder.Entity<EmployeeNotificationGroup>()
		        .HasOne(ent => ent.Employee)
		        .WithMany(e => e.EmployeeNotificationGroups)
		        .HasForeignKey(ent => ent.EmployeeId);

			builder.Entity<EmployeeNotificationGroup>()
				.HasOne(ent => ent.NotificationGroup)
				.WithMany(ng => ng.EmployeeNotificationGroups)
				.HasForeignKey(ent => ent.NotificationGroupId);

			// Comments
			builder.Entity<JobComment>()
            .HasKey(jc => jc.CommentId);

            builder.Entity<JobComment>()
                .HasOne(jc => jc.Job)
                .WithMany(j => j.Comments)
                .HasForeignKey(jc => jc.ParentId);

            builder.Entity<JobComment>()
                .HasOne(jc => jc.Owner)
                .WithMany()
                .HasForeignKey(jc => jc.OwnerID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<NoticeComment>()
            .HasKey(nc => nc.CommentId);

            builder.Entity<NoticeComment>()
                .HasOne(nc => nc.Notice)
                .WithMany(n => n.Comments)
                .HasForeignKey(nc => nc.ParentId);

            builder.Entity<NoticeComment>()
                .HasOne(nc => nc.Owner)
                .WithMany()
                .HasForeignKey(nc => nc.OwnerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Notifications
            builder.Entity<NoticeNotification>()
                .HasOne(nn => nn.NotificationGroup)
                .WithMany(ng => ng.NoticeNotifications)
                .HasForeignKey(nn => nn.NotificationGroupId)
                .IsRequired(true);

            builder.Entity<NotificationGroup>()
                .HasMany(ng => ng.NoticeNotifications)
                .WithOne(nn => nn.NotificationGroup)
                .IsRequired(false);

            base.OnModelCreating(builder);
		}

		public DbSet<Job> Jobs { get; set; } = default!;
        public DbSet<Employee> Employees { get; set; } = default!;
        public DbSet<Team> Teams { get; set; } = default!;
        public DbSet<Project> Projects { get; set; } = default!;
        public DbSet<Notice> Notices { get; set; } = default!;
        public DbSet<TeamProject> TeamProjects { get; set; } = default!;
        public DbSet<EmployeeProject> EmployeesProjects { get; set; } = default!;
        public DbSet<EmployeeTeam> EmployeeTeams { get; set; } = default!;
        public DbSet<JobComment> JobComments { get; set; } = default!;
        public DbSet<NoticeComment> NoticeComments { get; set; } = default!;
        public DbSet<Notification> Notifications { get; set;} = default!;
        public DbSet<NoticeNotification> NoticeNotifications { get; set; } = default!;
        public DbSet<JobNotification> JobNotifications { get; set; } = default!;
        public DbSet<JobCommentNotification> JobCommentsNotifications { get; set; } = default!;
        public DbSet<NoticeCommentNotification> NoticeCommentNotifications { get; set; } = default!;
        public DbSet<NotificationGroup> NotificationGroups { get; set; } = default!;
        public DbSet<EmployeeNotificationGroup> EmployeeNotificationGroups { get; set; } = default!;
    }
}