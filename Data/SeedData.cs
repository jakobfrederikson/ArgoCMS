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

                var empHenryID = await EnsureUser(serviceProvider, testUserPw, "henry@argo.com",
                                        "Henry", "Callaghan", 1);
                await EnsureRole(serviceProvider, empHenryID, Constants.EmployeesRole);

                var empLarryID = await EnsureUser(serviceProvider, testUserPw, "larry@argo.com",
                                        "Larry", "GPT", 1);
                await EnsureRole(serviceProvider, empLarryID, Constants.EmployeesRole);

                SeedDB(serviceProvider, context, adminID, vanahID, 
                    managerID, empGeorgeID, empJerryID, empHenryID, empLarryID);
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

            Team devTeam = new Team
            {
                TeamName = "Development Team",
                TeamDescription = "Consists of a few developers in the fake company.",
                DateCreated = DateTime.Now
            };

            Team pdTeam = new Team
            {
                TeamName = "Product Design Team",
                TeamDescription = "For those of the more creative side.",
                DateCreated = DateTime.Now
            };

            var teams = new Team[] { devTeam, pdTeam };
            context.Teams.AddRange(teams);
            await context.SaveChangesAsync();
        }

        public static async void SeedDB(IServiceProvider serviceProvider, ApplicationDbContext context,
            string adminID, string vanahID, string managerID, string empGeorgeID, string empJerryID,
            string empHenryID, string empLarryID)
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
            Employee henry = context.Employees.FirstOrDefault(e => e.Id == empHenryID);
            Employee larry = context.Employees.FirstOrDefault(e => e.Id == empLarryID);

            Team devTeam = context.Teams.FirstOrDefault(t => t.TeamId == 1);
            Team pdTeam = context.Teams.FirstOrDefault(t => t.TeamId == 2);

            devTeam.OwnerID = jakob.Id;
            pdTeam.OwnerID = nathan.Id;
            devTeam.Employees = new List<Employee> { jakob, vanah, henry, larry };
            pdTeam.Employees = new List<Employee> { nathan, george, jerry };

            // update ReportTo IDs
            jakob.ReportsToId = vanahID;
            vanah.ReportsToId = adminID;
            nathan.ReportsToId = vanahID;
            george.ReportsToId = adminID;
            jerry.ReportsToId = adminID;
            henry.ReportsToId = adminID;
            larry.ReportsToId = vanahID;

            Project paySys = new Project
            {
                ProjectName = "Pay System 2.0",
                OwnerID = vanahID,
                ProjectDescription = "Introduce a new internal pay system, replacing the old Java backend with .NET",
                DateCreated = DateTime.Now,
                Employees = new List<Employee>
                {
                    jakob,
                    vanah
                }
            };

            Project websiteUIRefactor = new Project
            {
                ProjectName = "Website UI Refactor",
                OwnerID = adminID,
                ProjectDescription = "Implement a modern frontend framework to the website, replacing old basic HTML code with React and magical frotend stuff.",
                DateCreated = DateTime.Now,
                Employees = new List<Employee>
                {
                    jakob,
                    jerry,
                    george
                }
            };

            var projects = new Project[] { paySys, websiteUIRefactor };
            context.Projects.AddRange(projects);

            Job paySysJob1 = new Job
            {
                OwnerID = adminID,
                EmployeeID = vanahID,
                TeamID = 1,
                JobName = "Withdraw System",
                JobDescription = "Convert the old withdraw function in Java to the new C# version.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Submitted,
                PriorityLevel = PriorityLevel.High
            };

            Job paySysJob2 = new Job
            {
                OwnerID = vanahID,
                EmployeeID = adminID,
                TeamID = 1,
                JobName = "Define Pay System Requirements",
                JobDescription = "Gather and document the specific requirements for the new pay system, including features, calculations, and reporting.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Submitted,
                PriorityLevel = PriorityLevel.High
            };

            Job paySysJob3 = new Job
            {
                OwnerID = vanahID,
                EmployeeID = adminID,
                TeamID = 1,
                JobName = "Research Payroll Regulations",
                JobDescription = "Research and compile information on local and federal payroll regulations to ensure compliance in the new pay system.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Completed,
                PriorityLevel = PriorityLevel.High
            };

            Job paySysJob4 = new Job
            {
                OwnerID = vanahID,
                EmployeeID = adminID,
                TeamID = 1,
                JobName = "Develop Payroll Database Schema",
                JobDescription = "Design the database structure for storing payroll data within the new pay system.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Completed,
                PriorityLevel = PriorityLevel.High
            };

            Job paySysJob5 = new Job
            {
                OwnerID = vanahID,
                EmployeeID = adminID,
                TeamID = 1,
                JobName = "Create Payroll Calculation Algorithms",
                JobDescription = "Develop algorithms to calculate employee salaries, taxes, and deductions in the new pay system.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Unread,
                PriorityLevel = PriorityLevel.High
            };

            Job paySysJob6 = new Job
            {
                OwnerID = adminID,
                EmployeeID = vanahID,
                TeamID = 1,
                JobName = "Design User Interface",
                JobDescription = "Create a user-friendly interface for the new pay system, allowing easy navigation and data input.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Completed,
                PriorityLevel = PriorityLevel.High
            };

            Job paySysJob7 = new Job
            {
                OwnerID = adminID,
                EmployeeID = vanahID,
                TeamID = 1,
                JobName = "Test Payroll Calculations",
                JobDescription = "Conduct testing to verify the accuracy of payroll calculations in the new pay system.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Completed,
                PriorityLevel = PriorityLevel.High
            };

            Job paySysJob8 = new Job
            {
                OwnerID = adminID,
                EmployeeID = vanahID,
                TeamID = 1,
                JobName = "Set Up Employee Benefits",
                JobDescription = "Configure and implement employee benefits programs within the new pay system.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Working,
                PriorityLevel = PriorityLevel.High
            };

            Job paySysJob9 = new Job
            {
                OwnerID = vanahID,
                EmployeeID = adminID,
                TeamID = 1,
                JobName = "Develop Training Materials",
                JobDescription = "Create training materials and documentation to educate users on using the new pay system.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Completed,
                PriorityLevel = PriorityLevel.High
            };

            Job paySysJob10 = new Job
            {
                OwnerID = vanahID,
                EmployeeID = adminID,
                TeamID = 1,
                JobName = "Perform User Acceptance Testing",
                JobDescription = "Engage users in testing the new pay system to ensure it meets their needs and expectations.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Read,
                PriorityLevel = PriorityLevel.High
            };

            Job paySysJob11 = new Job
            {
                OwnerID = vanahID,
                EmployeeID = adminID,
                TeamID = 1,
                JobName = "Plan System Rollout",
                JobDescription = "Develop a strategy for the smooth rollout of the new pay system, including employee training and communication plans.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Submitted,
                PriorityLevel = PriorityLevel.High
            };

            Job paySysJob12 = new Job
            {
                OwnerID = adminID,
                EmployeeID = henry.Id,
                TeamID = 1,
                JobName = "Data Migration and Integration",
                JobDescription = "Plan and execute the migration of existing payroll data into the new pay system while ensuring data integrity and system integration.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Completed,
                PriorityLevel = PriorityLevel.High
            };

            Job paySysJob13 = new Job
            {
                OwnerID = adminID,
                EmployeeID = henry.Id,
                TeamID = 1,
                JobName = "Security and Access Control",
                JobDescription = "Implement robust security measures and access controls to safeguard sensitive payroll information within the new pay system.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Unread,
                PriorityLevel = PriorityLevel.High
            };

            Job paySysJob14 = new Job
            {
                OwnerID = vanahID,
                EmployeeID = larry.Id,
                TeamID = 1,
                JobName = "Generate Payroll Reports",
                JobDescription = "Develop reporting functionalities in the pay system to generate various payroll reports for management and auditing purposes.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Completed,
                PriorityLevel = PriorityLevel.High
            };

            Job paySysJob15 = new Job
            {
                OwnerID = vanahID,
                EmployeeID = larry.Id,
                TeamID = 1,
                JobName = "Documentation and Knowledge Transfer",
                JobDescription = "Create comprehensive documentation for the new pay system, facilitating knowledge transfer among team members and future maintenance.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                JobStatus = JobStatus.Working,
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

            var jobs = new Job[] { 
                paySysJob1, 
                paySysJob2, 
                paySysJob3,
                paySysJob4,
                paySysJob5,
                paySysJob6,
                paySysJob7,
                paySysJob8,
                paySysJob9,
                paySysJob10,
                paySysJob11,
                paySysJob12,
                paySysJob13,
                paySysJob14,
                paySysJob15,
                createSeedClass };
            context.Jobs.AddRange(jobs);

            Notice notice1 = new Notice
            {
                TeamId = 1,
                OwnerID = adminID,
                NoticeTitle = "New Software Release",
                NoticeMessageContent = "The latest version of our software is now available for testing. Please review the release notes and report any issues.",
                DateCreated = DateTime.Now,
                PublicityStatus = PublicityStatus.OnlyTeam
            };

            Notice notice2 = new Notice
            {
                TeamId = 1,
                OwnerID = adminID,
                NoticeTitle = "Coding Standards Update",
                NoticeMessageContent = "We've updated our coding standards document. All developers should familiarize themselves with the changes and apply them in their work.",
                DateCreated = DateTime.Now,
                PublicityStatus = PublicityStatus.OnlyTeam
            };

            Notice notice3 = new Notice
            {
                TeamId = 1,
                OwnerID = adminID,
                NoticeTitle = "Development Team Meeting",
                NoticeMessageContent = "Reminder of the development team meeting scheduled for Friday at 10 AM to discuss project progress and upcoming tasks.",
                DateCreated = DateTime.Now,
                PublicityStatus = PublicityStatus.OnlyTeam
            };

            Notice notice4 = new Notice
            {
                TeamId = 2,
                OwnerID = managerID,
                NoticeTitle = "User Interface Redesign Project Kickoff",
                NoticeMessageContent = "We're excited to announce the kickoff of the UI redesign project. Let's collaborate and bring fresh ideas to the table!",
                DateCreated = DateTime.Now,
                PublicityStatus = PublicityStatus.OnlyTeam
            };

            Notice notice5 = new Notice
            {
                TeamId = 2,
                OwnerID = managerID,
                NoticeTitle = "Design Review Meeting",
                NoticeMessageContent = "There will be a design review meeting on Wednesday at 2 PM to ensure consistency and quality in our design work.",
                DateCreated = DateTime.Now,
                PublicityStatus = PublicityStatus.OnlyTeam
            };

            Notice notice6 = new Notice
            {
                TeamId = 2,
                OwnerID = managerID,
                NoticeTitle = "Creative Inspiration Workshop",
                NoticeMessageContent = "Join us for a creative inspiration workshop next week to explore new design trends and brainstorm innovative ideas.",
                DateCreated = DateTime.Now,
                PublicityStatus = PublicityStatus.OnlyTeam
            };

            Notice notice7 = new Notice
            {
                OwnerID = vanahID,
                NoticeTitle = "Employee Recognition Awards",
                NoticeMessageContent = "It's time to nominate outstanding colleagues for our Employee Recognition Awards. Let's celebrate and appreciate our team members.",
                DateCreated = DateTime.Now,
                PublicityStatus = PublicityStatus.Everyone
            };

            Notice notice8 = new Notice
            {
                OwnerID = managerID,
                NoticeTitle = "Company Quarterly Update",
                NoticeMessageContent = "Join us for our quarterly company-wide update meeting on [Date] to get insights on our progress and upcoming initiatives.",
                DateCreated = DateTime.Now,
                PublicityStatus = PublicityStatus.Everyone
            };

            Notice notice9 = new Notice
            {
                OwnerID = managerID,
                NoticeTitle = "Upcoming Holiday Office Closure",
                NoticeMessageContent = "Our offices will be closed on the 25th of December for Christmas. Please plan your work accordingly, and enjoy the holiday!",
                DateCreated = DateTime.Now,
                PublicityStatus = PublicityStatus.Everyone
            };

            var notices = new Notice[]
            {
                notice1,
                notice2,
                notice3,
                notice4,
                notice5,
                notice6,
                notice7,
                notice8,
                notice9
            };
            context.Notices.AddRange(notices);
            context.SaveChanges();
        }
    }
}
