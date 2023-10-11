using ArgoCMS.Models;
using Constants = ArgoCMS.Authorization.AuthorizationOperations.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ArgoCMS.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw = "")
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                EnsureTeams(context);

                // Create Users
                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@argo.com",
                                        "Jakob", "Frederikson", 1);
                await EnsureRole(serviceProvider, adminID, Constants.AdministratorsRole);

                var vanahID = await EnsureUser(serviceProvider, testUserPw, "vanah@argo.com",
                                        "Lavanah", "Holsted", 1);
                await EnsureRole(serviceProvider, vanahID, Constants.AdministratorsRole);

                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@argo.com",
                                        "Nathan", "Contoso", 2);
                await EnsureRole(serviceProvider, managerID, Constants.ManagersRole);

                var empGeorgeID = await EnsureUser(serviceProvider, testUserPw, "george@argo.com",
                                        "George", "Foreman", 2);
                await EnsureRole(serviceProvider, empGeorgeID, Constants.EmployeesRole);

                var empJerryID = await EnsureUser(serviceProvider, testUserPw, "jerry@argo.com",
                                        "Jerry", "Thompson", 2);
                await EnsureRole(serviceProvider, empJerryID, Constants.EmployeesRole);

                SeedDB(serviceProvider, context, adminID, vanahID, managerID, empGeorgeID, empJerryID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string UserName,
                                            string firstName, string lastName,
                                            int teamID)
        {
            var userManager = serviceProvider.GetService<UserManager<Employee>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new Employee
                {
                    FirstName = firstName,
                    LastName = lastName,
                    TeamID = teamID,
                    EmploymentDate = DateTime.Now,
                    UserName = UserName,
                    EmailConfirmed = true,
                    PersonalEmail = firstName + "@gmail.com"
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                              string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<Employee>>();

            //if (userManager == null)
            //{
            //    throw new Exception("userManager is null");
            //}

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

        private static async void EnsureTeams(ApplicationDbContext context)
        {
            if (context.Teams.Any())
            {
                return; // Teams already been seeded
            }

            Team swagTeam = new Team
            {
                TeamName = "Swag Team",
                TeamDescription = "For the swaggiest members of the company",
                DateCreated = DateTime.Now
            };

            Team coolTeam = new Team
            {
                TeamName = "Cool Guys",
                TeamDescription = "For the coolest of the company",
                DateCreated = DateTime.Now
            };

            var teams = new Team[] { swagTeam, coolTeam };
            context.Teams.AddRange(teams);
            await context.SaveChangesAsync();
        }

        public static async void SeedDB(IServiceProvider serviceProvider, ApplicationDbContext context,
            string adminID, string vanahID, string managerID, string empGeorgeID, string empJerryID)
        {
            if (context.Jobs.Any())
            {
                return;   // DB has been seeded
            }
            var userManager = serviceProvider.GetRequiredService<UserManager<Employee>>();
            Employee jakob = context.Employees.FirstOrDefault(e => e.Id == adminID);
            Employee vanah = context.Employees.FirstOrDefault(e => e.Id == vanahID);
            Employee nathan = context.Employees.FirstOrDefault(e => e.Id == managerID);
            Employee george = context.Employees.FirstOrDefault(e => e.Id == empGeorgeID);
            Employee jerry = context.Employees.FirstOrDefault(e => e.Id == empJerryID);

            Team swagTeam = context.Teams.FirstOrDefault(t => t.TeamId == 1);
            Team coolTeam = context.Teams.FirstOrDefault(t => t.TeamId == 2);

            swagTeam.OwnerID = jakob.Id;
            coolTeam.OwnerID = nathan.Id;
            swagTeam.Employees = new List<Employee> { jakob, vanah };
            coolTeam.Employees = new List<Employee> { nathan, george, jerry };

            // update ReportTo IDs
            jakob.ReportsToId = vanahID;
            vanah.ReportsToId = adminID;
            nathan.ReportsToId = vanahID;
            george.ReportsToId = adminID;
            jerry.ReportsToId = adminID;

            Project swagProject = new Project
            {
                ProjectName = "Swag Project",
                OwnerID = vanahID,
                ProjectDescription = "This project is to make sure a cool S is drawn on every desk in the office.",
                DateCreated = DateTime.Now,
                Employees = new List<Employee>
                {
                    jakob,
                    vanah
                }
            };

            Project hardProject = new Project
            {
                ProjectName = "Create backend for CMS",
                OwnerID = adminID,
                ProjectDescription = "This project is hard but fun",
                DateCreated = DateTime.Now,
                Employees = new List<Employee>
                {
                    jakob,
                    jerry,
                    george
                }
            };

            var projects = new Project[] { swagProject, hardProject };
            context.Projects.AddRange(projects);

            Job swagJob1 = new Job
            {
                OwnerID = adminID,
                EmployeeID = vanahID,
                TeamID = 1,
                JobName = "Vanah needs to draw a cool S on Jerry's desk",
                JobDescription = "Jerry doesn't have a cool S on his desk and this needs to be taken care of immediately.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Submitted,
                PriorityLevel = PriorityLevel.High
            };

            Job swagJob2 = new Job
            {
                OwnerID = vanahID,
                EmployeeID = adminID,
                TeamID = 1,
                JobName = "Jakob needs to draw a cool S on George's desk",
                JobDescription = "George doesn't have a cool S on his desk and this needs to be taken care of immediately.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Submitted,
                PriorityLevel = PriorityLevel.High
            };

            Job createSeedClass = new Job
            {
                OwnerID = adminID,
                EmployeeID = empJerryID,
                TeamID = 2,
                JobName = "Create DB seed data",
                JobDescription = "Create a seed object that can be called in Program.cs",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Submitted,
                PriorityLevel = PriorityLevel.Medium
            };

            var jobs = new Job[] { swagJob1, swagJob2, createSeedClass };
            context.Jobs.AddRange(jobs);

            context.SaveChanges();
        }
    }
}
