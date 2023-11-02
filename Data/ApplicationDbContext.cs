using ArgoCMS.Models;
using ArgoCMS.Models.JointEntities;
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

            // Employees
            builder.Entity<Employee>()
                .HasOne(e => e.Team)
                .WithMany()
                .HasForeignKey(e => e.TeamID);

            builder.Entity<Employee>()
                .HasOne(e => e.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(e => e.TeamID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Employee>()
                .HasMany(e => e.Jobs)
                .WithOne(j => j.AssignedEmployee)
                .HasForeignKey(e => e.AssignedEmployeeID);

            // Teams
            builder.Entity<Team>()
                .HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedById);

            builder.Entity<Team>()
                .HasOne(t => t.TeamLeader)
                .WithMany()
                .HasForeignKey(t => t.TeamLeaderId);

            // Projects
            builder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany()
                .HasForeignKey(p => p.OwnerID)
                .OnDelete(DeleteBehavior.Restrict);

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
			.HasKey(ep => new { ep.EmployeeId, ep.ProjectId });

			builder.Entity<EmployeeProject>()
				.HasOne(ep => ep.Employee)
				.WithMany(e => e.EmployeeProjects)
				.HasForeignKey(ep => ep.EmployeeId);

			builder.Entity<EmployeeProject>()
				.HasOne(ep => ep.Project)
				.WithMany(p => p.EmployeeProjects)
				.HasForeignKey(ep => ep.ProjectId);

            // Comments
            builder.Entity<JobComment>()
            .HasKey(jc => jc.CommentId);

            builder.Entity<JobComment>()
                .HasOne(jc => jc.Job)
                .WithMany(j => j.Comments)
                .HasForeignKey(jc => jc.ParentId);

            builder.Entity<NoticeComment>()
            .HasKey(nc => nc.CommentId);

            builder.Entity<NoticeComment>()
                .HasOne(nc => nc.Notice)
                .WithMany(n => n.Comments)
                .HasForeignKey(nc => nc.ParentId);

            base.OnModelCreating(builder);
		}

		public DbSet<Job> Jobs { get; set; } = default!;
        public DbSet<Employee> Employees { get; set; } = default!;
        public DbSet<Team> Teams { get; set; } = default!;
        public DbSet<Project> Projects { get; set; } = default!;
        public DbSet<Notice> Notices { get; set; } = default!;
        public DbSet<TeamProject> TeamProjects { get; set; } = default!;
        public DbSet<EmployeeProject> EmployeesProjects { get; set; } = default!;
        public DbSet<JobComment> JobComments { get; set; } = default!;
        public DbSet<NoticeComment> NoticeComments { get; set; } = default!;
    }
}