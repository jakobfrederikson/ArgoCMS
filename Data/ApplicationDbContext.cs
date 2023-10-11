using ArgoCMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; } = default!;
        public DbSet<Employee> Employees { get; set; } = default!;
        public DbSet<Team> Teams { get; set; } = default!;
        public DbSet<Project> Projects { get; set; } = default!;
        public DbSet<Notice> Notices { get; set; }
    }
}