using ArgoCMS.Models;
using Constants = ArgoCMS.Authorization.AuthorizationOperations.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Models.JointEntities;
using Newtonsoft.Json.Linq;

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
            context.SaveChanges();
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

            devTeam.TeamLeaderId = jakob.Id;
            devTeam.CreatedById = jakob.Id;
            pdTeam.TeamLeaderId = nathan.Id;
            pdTeam.CreatedById = nathan.Id;
            devTeam.Members = new List<Employee> { jakob, vanah, henry, larry };
            pdTeam.Members = new List<Employee> { nathan, george, jerry };

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
                DueDate = DateTime.Now.AddDays(3),
                EmployeeProjects = new List<EmployeeProject>
                {
					new EmployeeProject { Employee = jakob, IsCompleted = false },
                    new EmployeeProject { Employee = vanah, IsCompleted = false }
				}
            };

            Project websiteUIRefactor = new Project
            {
                ProjectName = "Website UI Refactor",
                OwnerID = adminID,
                ProjectDescription = "Implement a modern frontend framework to the website, replacing old basic HTML code with React and magical frotend stuff.",
                DateCreated = DateTime.Now,
				DueDate = DateTime.Now.AddDays(3),
                ProjectStatus = ProjectStatus.InProgress,
				EmployeeProjects = new List<EmployeeProject>
                {
					new EmployeeProject { Employee = jakob, IsCompleted = false },
		            new EmployeeProject { Employee = jerry, IsCompleted = false },
		            new EmployeeProject { Employee = george, IsCompleted = false }
				}
            };

            Project devTeamProject = new Project
            {
                OwnerID = adminID,
                ProjectName = "Dev Team Project 1",
                ProjectDescription = "Develop new features for the software",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddMonths(3),
                ProjectStatus = ProjectStatus.InProgress,
                EmployeeProjects = new List< EmployeeProject>
                {
                    new EmployeeProject { Employee = jakob, IsCompleted = false },
                    new EmployeeProject { Employee = vanah, IsCompleted = false },
                    new EmployeeProject { Employee = henry, IsCompleted = false },
                    new EmployeeProject { Employee = larry, IsCompleted = false }
                }
            };

            Project designTeamProject = new Project
            {
                OwnerID = managerID,
                ProjectName = "Design Team Project 1",
                ProjectDescription = "Create user interface designs for the new product",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddMonths(2),
                ProjectStatus = ProjectStatus.NotStarted,
                EmployeeProjects = new List<EmployeeProject>
                {
                    new EmployeeProject { Employee = nathan, IsCompleted = false },
                    new EmployeeProject { Employee = george, IsCompleted = false },
                    new EmployeeProject { Employee = jerry, IsCompleted = false }
                }
            };

            var crossTeamProject = new Project
            {
                OwnerID = adminID,
                ProjectName = "Cross-Team Collaboration Project",
                ProjectDescription = "A project that involves both development and design teams working together.",
                DateCreated = DateTime.Now,
                DueDate = DateTime.Now.AddMonths(4),
                ProjectStatus = ProjectStatus.NotStarted,
                EmployeeProjects = new List<EmployeeProject>
                {
                    new EmployeeProject { Employee = jakob, IsCompleted = false },
                    new EmployeeProject { Employee = vanah, IsCompleted = false },
                    new EmployeeProject { Employee = henry, IsCompleted = false },
                    new EmployeeProject { Employee = larry, IsCompleted = false },
                    new EmployeeProject { Employee = nathan, IsCompleted = false },
                    new EmployeeProject { Employee = george, IsCompleted = false },
                    new EmployeeProject { Employee = jerry, IsCompleted = false }
                }
            };

            var finishedProject = new Project
            {
                OwnerID = adminID,
                ProjectName = "Finished Project",
                ProjectDescription = "Testing to see if this project comes up as completed",
                DateCreated = DateTime.Now.AddDays(-3),
                DueDate = DateTime.Now,
                ProjectStatus = ProjectStatus.Completed,
                EmployeeProjects = new List<EmployeeProject>
                {
                    new EmployeeProject { Employee = jakob, IsCompleted = true},
                    new EmployeeProject { Employee = vanah, IsCompleted = true},
                    new EmployeeProject { Employee = henry, IsCompleted = true},
                    new EmployeeProject { Employee = larry, IsCompleted = true},
                    new EmployeeProject { Employee = nathan, IsCompleted = true },
                    new EmployeeProject { Employee = george, IsCompleted = true },
                    new EmployeeProject { Employee = jerry, IsCompleted = true}
                }
            };

            var projects = new Project[] { devTeamProject, designTeamProject, paySys, websiteUIRefactor, crossTeamProject, finishedProject };
            context.Projects.AddRange(projects);
            context.SaveChanges();

            var teamProject1 = new TeamProject
            {
                TeamId = devTeam.TeamId,
                ProjectId = crossTeamProject.ProjectId
            };
            context.TeamProjects.Add(teamProject1);

            var teamProject2 = new TeamProject
            {
                TeamId = pdTeam.TeamId,
                ProjectId = crossTeamProject.ProjectId
            };
            context.TeamProjects.Add(teamProject2);

            var teamProject3 = new TeamProject
            {
                TeamId = devTeam.TeamId,
                ProjectId = devTeamProject.ProjectId
            };
            context.TeamProjects.Add(teamProject3);

            var teamProject4 = new TeamProject
            {
                TeamId = devTeam.TeamId,
                ProjectId = websiteUIRefactor.ProjectId
            };
            context.TeamProjects.Add(teamProject4);

            var teamProject5 = new TeamProject
            {
                TeamId = pdTeam.TeamId,
                ProjectId = websiteUIRefactor.ProjectId
            };
            context.TeamProjects.Add(teamProject5);

            context.SaveChanges();


            var jobs = new Job[]
            {
                new Job
                {
                    OwnerID = adminID,
                    AssignedEmployeeID = vanahID,
                    TeamID = 1,
                    JobName = "Code Review",
                    JobDescription = "Conduct a code review for the recent feature implementation. Ensure adherence to coding standards.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(7),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Medium
                },
                new Job
                {
                    OwnerID = empHenryID,
                    AssignedEmployeeID = empLarryID,
                    TeamID = 1,
                    JobName = "Documentation Update",
                    JobDescription = "Update the project documentation with the latest changes and improvements made to the application.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(10),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Low
                },
                new Job
                {
                    OwnerID = vanahID,
                    AssignedEmployeeID = adminID,
                    TeamID = 1,
                    JobName = "Documentation Review",
                    JobDescription = "Review the project documentation and ensure accuracy and completeness.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(5),
                    JobStatus = JobStatus.Working,
                    PriorityLevel = PriorityLevel.Low
                },
                new Job
                {
                    OwnerID = vanahID,
                    AssignedEmployeeID = adminID,
                    TeamID = 1,
                    JobName = "Code Cleanup",
                    JobDescription = "Clean up and optimize the codebase by removing unused code and improving code quality.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(7),
                    JobStatus = JobStatus.Working,
                    PriorityLevel = PriorityLevel.Normal
                },
                new Job
                {
                    OwnerID = adminID,
                    AssignedEmployeeID = empHenryID,
                    TeamID = 1,
                    JobName = "Bug Fixing",
                    JobDescription = "Investigate and fix reported bugs in the application. Ensure thorough testing.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(10),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Medium
                },
                new Job
                {
                    OwnerID = empLarryID,
                    AssignedEmployeeID = vanahID,
                    TeamID = 1,
                    JobName = "Feature Testing",
                    JobDescription = "Perform comprehensive testing of the new feature to identify and report any issues.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(8),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Normal
                },
                new Job
                {
                    OwnerID = adminID,
                    AssignedEmployeeID = empHenryID,
                    TeamID = 1,
                    JobName = "Performance Optimization",
                    JobDescription = "Optimize application performance by identifying and resolving bottlenecks.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(15),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.High
                },
                new Job
                {
                    OwnerID = empLarryID,
                    AssignedEmployeeID = vanahID,
                    TeamID = 1,
                    JobName = "Documentation Updates",
                    JobDescription = "Update the project documentation with new information and recent changes.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(9),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Normal
                },
                new Job
                {
                    OwnerID = empHenryID,
                    AssignedEmployeeID = adminID,
                    TeamID = 1,
                    JobName = "User Training",
                    JobDescription = "Conduct a training session for end-users to explain new features and best practices.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(12),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Medium
                },
                new Job
                {
                    OwnerID = managerID,
                    AssignedEmployeeID = empGeorgeID,
                    TeamID = 2,
                    JobName = "Feature Implementation",
                    JobDescription = "Implement a new feature based on the client's requirements. Ensure functionality and performance.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(21),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.High
                },
                new Job
                {
                    OwnerID = empJerryID,
                    AssignedEmployeeID = managerID,
                    TeamID = 2,
                    JobName = "Database Schema Changes",
                    JobDescription = "Make necessary changes to the database schema to accommodate new features. Ensure data migration.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Normal
                },
                new Job
                {
                    OwnerID = empGeorgeID,
                    AssignedEmployeeID = managerID,
                    TeamID = 2,
                    JobName = "Product Strategy Meeting",
                    JobDescription = "Schedule and prepare for a product strategy meeting to discuss future product directions.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(7),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Normal
                },
                new Job
                {
                    OwnerID = managerID,
                    AssignedEmployeeID = empJerryID,
                    TeamID = 2,
                    JobName = "Market Research",
                    JobDescription = "Conduct market research to identify trends and opportunities in the industry.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Medium
                },
                new Job
                {
                    OwnerID = empGeorgeID,
                    AssignedEmployeeID = managerID,
                    TeamID = 2,
                    JobName = "Product Design Review",
                    JobDescription = "Review and provide feedback on the latest product design concepts.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(10),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Normal
                },
                new Job
                {
                    OwnerID = empJerryID,
                    AssignedEmployeeID = managerID,
                    TeamID = 2,
                    JobName = "Development Task Assignment",
                    JobDescription = "Assign development tasks to team members for upcoming projects.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(7),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Low
                },
                new Job
                {
                    OwnerID = empGeorgeID,
                    AssignedEmployeeID = empJerryID,
                    TeamID = 2,
                    JobName = "User Experience Workshop",
                    JobDescription = "Organize a workshop to enhance the team's understanding of user experience principles.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(12),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.High
                },
                new Job
                {
                    OwnerID = managerID,
                    AssignedEmployeeID = empJerryID,
                    TeamID = 2,
                    JobName = "Development Progress Report",
                    JobDescription = "Create a progress report summarizing the status of ongoing development projects.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(9),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Normal
                },
                new Job
                {
                    OwnerID = empJerryID,
                    AssignedEmployeeID = empGeorgeID,
                    TeamID = 2,
                    JobName = "Design Prototyping",
                    JobDescription = "Work on design prototypes for the upcoming product features and improvements.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(11),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Medium
                },
                new Job
                {
                    OwnerID = managerID,
                    AssignedEmployeeID = empGeorgeID,
                    TeamID = 2,
                    JobName = "Market Analysis Report",
                    JobDescription = "Prepare a comprehensive analysis report of the current market landscape.",
                    DateCreated = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Normal
                }
            };
            context.Jobs.AddRange(jobs);
            context.SaveChanges();

            var jobComments = new JobComment[]
            {
                new JobComment
                {
                    ParentId = 1,
                    OwnerID = adminID,
                    CommentText = "I've reviewed the code, and it looks great! Just a minor suggestion for improvement.",
                    CreationDate = DateTime.Now.AddDays(-2)
                },
                new JobComment
                {
                    ParentId = 1,
                    OwnerID = vanahID,
                    CommentText = "Thank you for the review, Jakob! I've addressed your suggestion and updated the code.",
                    CreationDate = DateTime.Now.AddDays(-1)
                },
                new JobComment
                {
                    ParentId = 1,
                    OwnerID = adminID,
                    CommentText = "Excellent! The code now looks perfect. Let's mark this code review as completed.",
                    CreationDate = DateTime.Now.AddDays(-1)
                },
                new JobComment
                {
                    ParentId = 3,
                    OwnerID = vanahID,
                    CommentText = "Jakob, I've assigned you to review the latest documentation updates. Please provide your feedback when you can.",
                    CreationDate = DateTime.Now.AddDays(-3)
                },
                new JobComment
                {
                    ParentId = 3,
                    OwnerID = adminID,
                    CommentText = "Sure, I'll start reviewing the documentation right away and provide feedback soon.",
                    CreationDate = DateTime.Now.AddDays(-2)
                },
                new JobComment
                {
                    ParentId = 3,
                    OwnerID = vanahID,
                    CommentText = "Thank you, admin! I appreciate your prompt response.",
                    CreationDate = DateTime.Now.AddDays(-2)
                },
                new JobComment
                {
                    ParentId = 4,
                    OwnerID = vanahID,
                    CommentText = "Jakob, I've assigned you to handle the Code Cleanup task. The goal is to clean up and optimize the codebase by removing any unused code.",
                    CreationDate = DateTime.Now.AddDays(-3)
                },
                new JobComment
                {
                    ParentId = 4,
                    OwnerID = adminID,
                    CommentText = "Understood, I'll start the code cleanup process. I'll analyze the codebase and remove any redundant or unused code to optimize it.",
                    CreationDate = DateTime.Now.AddDays(-2)
                },
                new JobComment
                {
                    ParentId = 4,
                    OwnerID = vanahID,
                    CommentText = "Great, Jakob! Let me know if you need any assistance or have any questions during the code cleanup.",
                    CreationDate = DateTime.Now.AddDays(-2)
                }
            };

            context.JobComments.AddRange(jobComments);
            context.SaveChanges();

            var notices = new Notice[]
            {
                new Notice
                {
                    TeamId = 1,
                    OwnerID = adminID,
                    NoticeTitle = "New Software Release",
                    NoticeMessageContent = "The latest version of our software is now available for testing. Please review the release notes and report any issues.",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.OnlyTeam
                },
                new Notice
                {
                    TeamId = 1,
                    OwnerID = adminID,
                    NoticeTitle = "Coding Standards Update",
                    NoticeMessageContent = "We've updated our coding standards document. All developers should familiarize themselves with the changes and apply them in their work.",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.OnlyTeam
                },
                new Notice
                {
                    TeamId = 1,
                    OwnerID = adminID,
                    NoticeTitle = "Development Team Meeting",
                    NoticeMessageContent = "Reminder of the development team meeting scheduled for Friday at 10 AM to discuss project progress and upcoming tasks.",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.OnlyTeam
                },
                new Notice
                {
                    TeamId = 2,
                    OwnerID = managerID,
                    NoticeTitle = "User Interface Redesign Project Kickoff",
                    NoticeMessageContent = "We're excited to announce the kickoff of the UI redesign project. Let's collaborate and bring fresh ideas to the table!",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.OnlyTeam
                },
                new Notice
                {
                    TeamId = 2,
                    OwnerID = managerID,
                    NoticeTitle = "Design Review Meeting",
                    NoticeMessageContent = "There will be a design review meeting on Wednesday at 2 PM to ensure consistency and quality in our design work.",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.OnlyTeam
                },
                new Notice
                {
                    TeamId = 2,
                    OwnerID = managerID,
                    NoticeTitle = "Creative Inspiration Workshop",
                    NoticeMessageContent = "Join us for a creative inspiration workshop next week to explore new design trends and brainstorm innovative ideas.",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.OnlyTeam
                },
                new Notice
                {
                    OwnerID = vanahID,
                    NoticeTitle = "Employee Recognition Awards",
                    NoticeMessageContent = "It's time to nominate outstanding colleagues for our Employee Recognition Awards. Let's celebrate and appreciate our team members.",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.Everyone
                },
                new Notice
                {
                    OwnerID = managerID,
                    NoticeTitle = "Company Quarterly Update",
                    NoticeMessageContent = "Join us for our quarterly company-wide update meeting on [Date] to get insights on our progress and upcoming initiatives.",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.Everyone
                },
                new Notice
                {
                    OwnerID = managerID,
                    NoticeTitle = "Upcoming Holiday Office Closure",
                    NoticeMessageContent = "Our offices will be closed on the 25th of December for Christmas. Please plan your work accordingly, and enjoy the holiday!",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.Everyone
                },
            };
            context.Notices.AddRange(notices);
            context.SaveChanges();

            var comments = new NoticeComment[]
            {
                new NoticeComment
                {
                    ParentId = 1,
                    OwnerID = empHenryID,
                    CommentText = "I won't be able to attend this Friday's meeting, but I'll catch up with the minutes. Please make sure to discuss the latest project updates and any blockers.",
                    CreationDate = DateTime.Now,
                },
                new NoticeComment
                {
                    ParentId = 1,
                    OwnerID = vanahID,
                    CommentText = "Is it possible to consider shifting the meeting time to 2 PM? It would be more convenient for me due to another commitment in the morning.",
                    CreationDate = DateTime.Now,
                },
                new NoticeComment
                {
                    ParentId = 1,
                    OwnerID = empLarryID,
                    CommentText = "I'd like to suggest adding a discussion on the new development tools we've been exploring. It might be beneficial for the team's efficiency.",
                    CreationDate = DateTime.Now,
                },
                new NoticeComment
                {
                    ParentId = 4,
                    OwnerID = empGeorgeID,
                    CommentText = "Exciting news! I'm eager to contribute to the UI redesign project. Can we schedule an initial brainstorming session to share ideas and set project goals?",
                    CreationDate = DateTime.Now,
                },
                new NoticeComment
                {
                    ParentId = 4,
                    OwnerID = empJerryID,
                    CommentText = "I've been researching user experience trends and have some interesting insights to bring to the project. Looking forward to making our designs user-centric!",
                    CreationDate = DateTime.Now,
                },
                new NoticeComment
                {
                    ParentId = 4,
                    OwnerID = managerID,
                    CommentText = "It would be great to establish clear design guidelines and a shared design library from the beginning. Let's ensure consistency in our new UI elements.",
                    CreationDate = DateTime.Now,
                },
                new NoticeComment
                {
                    ParentId = 7,
                    OwnerID = adminID,
                    CommentText = "This is a fantastic initiative! Let's make sure to recognize the hard work and dedication of our colleagues. I'm excited to see the nominations.",
                    CreationDate = DateTime.Now,
                },
                new NoticeComment
                {
                    ParentId = 7,
                    OwnerID = managerID,
                    CommentText = "I've had the privilege to work with some outstanding colleagues. I'll be sure to nominate those who've gone above and beyond. Let's appreciate our fantastic team!",
                    CreationDate = DateTime.Now,
                },
                new NoticeComment
                {
                    ParentId = 7,
                    OwnerID = empGeorgeID,
                    CommentText = "As a new member of the team, I'm thrilled to be part of a company that values employee recognition. It sets a positive tone for the workplace, and I can't wait to participate in the process.",
                    CreationDate = DateTime.Now,
                },
            };

            context.NoticeComments.AddRange(comments);
            context.SaveChanges();
        }
    }
}
