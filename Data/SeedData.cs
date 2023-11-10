using ArgoCMS.Models;
using Constants = ArgoCMS.Authorization.AuthorizationOperations.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Models.JointEntities;
using Newtonsoft.Json.Linq;
using ArgoCMS.Models.Comments;
using ArgoCMS.Models.Notifications;

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

                // Create Users
                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@argo.com",
                                        "Jakob", "Frederikson");
                await EnsureRole(serviceProvider, adminID, Constants.AdministratorsRole);

                var mohammadID = await EnsureUser(serviceProvider, testUserPw, "mohammad@argo.com",
                                        "Mohammad", "Norouzifard");
                await EnsureRole(serviceProvider, mohammadID, Constants.AdministratorsRole);

                var arthurID = await EnsureUser(serviceProvider, testUserPw, "arthur@argo.com",
                                        "Arthur", "Lewis");
                await EnsureRole(serviceProvider, arthurID, Constants.AdministratorsRole);

                var vanahID = await EnsureUser(serviceProvider, testUserPw, "vanah@argo.com",
                                        "Lavanah", "Holsted");
                await EnsureRole(serviceProvider, vanahID, Constants.AdministratorsRole);

                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@argo.com",
                                        "Nathan", "Contoso");
                await EnsureRole(serviceProvider, managerID, Constants.ManagersRole);

                var empGeorgeID = await EnsureUser(serviceProvider, testUserPw, "george@argo.com",
                                        "George", "Foreman");
                await EnsureRole(serviceProvider, empGeorgeID, Constants.EmployeesRole);

                var empJerryID = await EnsureUser(serviceProvider, testUserPw, "jerry@argo.com",
                                        "Jerry", "Thompson");
                await EnsureRole(serviceProvider, empJerryID, Constants.EmployeesRole);

                var empHenryID = await EnsureUser(serviceProvider, testUserPw, "henry@argo.com",
                                        "Henry", "Callaghan");
                await EnsureRole(serviceProvider, empHenryID, Constants.EmployeesRole);

                var empLarryID = await EnsureUser(serviceProvider, testUserPw, "larry@argo.com",
                                        "Larry", "GPT");
                await EnsureRole(serviceProvider, empLarryID, Constants.EmployeesRole);

                SeedDB(serviceProvider, context, adminID, vanahID, 
                    managerID, empGeorgeID, empJerryID, empHenryID, empLarryID,
                    mohammadID, arthurID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string UserName,
                                            string firstName, string lastName)
        {
            var userManager = serviceProvider.GetService<UserManager<Employee>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new Employee
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EmploymentDate = DateTime.Now,
                    UserName = UserName,
                    EmailConfirmed = true,
                    PersonalEmail = firstName + "@gmail.com",
                    Email = UserName,
                    PhoneNumber = "123456789",
                    PhoneNumberConfirmed = true
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

        public static async void SeedDB(IServiceProvider serviceProvider, ApplicationDbContext context,
            string adminID, string vanahID, string managerID, string empGeorgeID, string empJerryID,
            string empHenryID, string empLarryID, string mohammadID, string arthurID)
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
            Employee mohammad = context.Employees.FirstOrDefault(e => e.Id == mohammadID);
            Employee arthur = context.Employees.FirstOrDefault(e => e.Id == arthurID);

            // update ReportTo IDs
            jakob.ReportsToId = vanahID;
            vanah.ReportsToId = adminID;
            nathan.ReportsToId = vanahID;
            george.ReportsToId = adminID;
            jerry.ReportsToId = adminID;
            henry.ReportsToId = adminID;
            larry.ReportsToId = vanahID;
            mohammad.ReportsToId = arthurID;
            arthur.ReportsToId = mohammadID;

            Team devTeam = new Team
            {
                CreatedBy = jakob,
                TeamLeader = jakob,
                TeamName = "Development Team",
                TeamDescription = "Consists of a few developers in the fake company.",
                DateCreated = DateTime.Now
            };

            Team pdTeam = new Team
            {
                CreatedBy = vanah,
                TeamLeader = nathan,
                TeamName = "Product Design Team",
                TeamDescription = "For those of the more creative side.",
                DateCreated = DateTime.Now
            };

            Team YoobeeTeam = new Team
            {
                CreatedBy = mohammad,
                TeamLeader = mohammad,
                TeamName = "Yoobee Team",
                TeamDescription = "For my tutors that have helped me on this project.",
                DateCreated = DateTime.Now
            };

            devTeam.Members = new List<Employee> { jakob, vanah, henry, larry };
            pdTeam.Members = new List<Employee> { jakob, nathan, george, jerry };
            YoobeeTeam.Members = new List<Employee> { mohammad, arthur, jakob };

            context.Teams.AddRange(new Team[] { devTeam, pdTeam, YoobeeTeam });
            context.SaveChanges();
            
            var ets = new EmployeeTeam[]
            {
                new EmployeeTeam
                {
                    Employee = jakob,
                    Team = devTeam
                },
                new EmployeeTeam
                {
                    Employee = vanah,
                    Team = devTeam
                },
                new EmployeeTeam
                {
                    Employee = henry,
                    Team = devTeam
                },
                new EmployeeTeam
                {
                    Employee = larry,
                    Team = devTeam
                },
                new EmployeeTeam
                {
                    Employee = jerry,
                    Team = pdTeam
                },
                new EmployeeTeam
                {
                    Employee = george,
                    Team = pdTeam
                },
                new EmployeeTeam
                {
                    Employee = nathan,
                    Team = pdTeam
                },
                new EmployeeTeam
                {
                    Employee = jakob,
                    Team = pdTeam
                },
                new EmployeeTeam
                {
                    Employee = mohammad,
                    Team = YoobeeTeam
                },
                new EmployeeTeam
                {
                    Employee = arthur,
                    Team = YoobeeTeam
                },
                new EmployeeTeam
                {
                    Employee = jakob,
                    Team = YoobeeTeam
                }
        };

            context.EmployeeTeams.AddRange(ets);
            context.SaveChanges(); 

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
                    JobStatus = JobStatus.Working,
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
                },
                new Job
                {
                    OwnerID = vanahID,
                    AssignedEmployeeID = adminID,
                    TeamID = 1,
                    JobName = "Meet with Product Design",
                    JobDescription = "Please meet with the product design team and discuss some new features for the app.",
                    DateCreated = DateTime.Now.AddDays(-3),
                    DueDate = DateTime.Now.AddDays(-2),
                    JobStatus = JobStatus.Completed,
                    PriorityLevel = PriorityLevel.Normal
                },
                new Job
                {
                    OwnerID = vanahID,
                    AssignedEmployeeID = adminID,
                    TeamID = 1,
                    JobName = "Get Coffee for the Team",
                    JobDescription = "Get some coffee for development team tomorrow morning :)",
                    DateCreated = DateTime.Now.AddDays(-1),
                    DueDate = DateTime.Now,
                    JobStatus = JobStatus.Completed,
                    PriorityLevel = PriorityLevel.Normal
                },
                new Job
                {
                    OwnerID = managerID,
                    AssignedEmployeeID = adminID,
                    TeamID = 1,
                    JobName = "Dashboard UI Meeting",
                    JobDescription = "Hi Jakob, come meet me at some point and we'll have a meeting to discuss implementing the new UI.",
                    DateCreated = DateTime.Now.AddDays(-5),
                    DueDate = DateTime.Now.AddDays(-4),
                    JobStatus = JobStatus.Completed,
                    PriorityLevel = PriorityLevel.Normal
                },
                new Job
                {
                    OwnerID = adminID,
                    AssignedEmployeeID = vanahID,
                    TeamID = 1,
                    JobName = "Get us Coffee!",
                    JobDescription = "We need some coffee tomorrow, thanks!",
                    DateCreated = DateTime.Now.AddDays(-5),
                    DueDate = DateTime.Now.AddDays(-4),
                    JobStatus = JobStatus.Completed,
                    PriorityLevel = PriorityLevel.Normal
                },
                new Job
                {
                    OwnerID = adminID,
                    AssignedEmployeeID = vanahID,
                    TeamID = 1,
                    JobName = "Design New Feature",
                    JobDescription = "There's a feature that needs implementing, can you design it?",
                    DateCreated = DateTime.Now.AddDays(-7),
                    DueDate = DateTime.Now.AddDays(-5),
                    JobStatus = JobStatus.Completed,
                    PriorityLevel = PriorityLevel.Normal
                },
                new Job
                {
                    OwnerID = adminID,
                    AssignedEmployeeID = vanahID,
                    TeamID = 1,
                    JobName = "New Project Meeting",
                    JobDescription = "Let's discuss creating a new project.",
                    DateCreated = DateTime.Now.AddDays(-8),
                    DueDate = DateTime.Now.AddDays(-7),
                    JobStatus = JobStatus.Completed,
                    PriorityLevel = PriorityLevel.Normal
                },
                new Job
                {
                    OwnerID = mohammadID,
                    AssignedEmployeeID = adminID,
                    TeamID = YoobeeTeam.TeamId,
                    JobName = "Finish your Capstone Project",
                    JobDescription = "What are you doing? Hurry up!",
                    DateCreated = DateTime.Now,
                    DueDate = new DateTime(2023, 11, 24),
                    JobStatus = JobStatus.Working,
                    PriorityLevel = PriorityLevel.Critical
                },
                new Job
                {
                    OwnerID = arthurID,
                    AssignedEmployeeID = mohammadID,
                    TeamID = YoobeeTeam.TeamId,
                    JobName = "Take a break",
                    JobDescription = "You've been working hard, you should take a holiday.",
                    DateCreated = DateTime.Now,
                    DueDate = new DateTime(2023, 11, 24),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Medium
                },
                new Job
                {
                    OwnerID = mohammadID,
                    AssignedEmployeeID = arthurID,
                    TeamID = YoobeeTeam.TeamId,
                    JobName = "Take a break",
                    JobDescription = "Hey, you should also take a break!",
                    DateCreated = DateTime.Now,
                    DueDate = new DateTime(2023, 11, 24),
                    JobStatus = JobStatus.Unread,
                    PriorityLevel = PriorityLevel.Medium
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
                    PublicityStatus = PublicityStatus.Team
                },
                new Notice
                {
                    TeamId = 1,
                    OwnerID = adminID,
                    NoticeTitle = "Coding Standards Update",
                    NoticeMessageContent = "We've updated our coding standards document. All developers should familiarize themselves with the changes and apply them in their work.",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.Team
                },
                new Notice
                {
                    TeamId = 1,
                    OwnerID = adminID,
                    NoticeTitle = "Development Team Meeting",
                    NoticeMessageContent = "Reminder of the development team meeting scheduled for Friday at 10 AM to discuss project progress and upcoming tasks.",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.Team
                },
                new Notice
                {
                    TeamId = 2,
                    OwnerID = managerID,
                    NoticeTitle = "User Interface Redesign Project Kickoff",
                    NoticeMessageContent = "We're excited to announce the kickoff of the UI redesign project. Let's collaborate and bring fresh ideas to the table!",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.Team
                },
                new Notice
                {
                    TeamId = 2,
                    OwnerID = managerID,
                    NoticeTitle = "Design Review Meeting",
                    NoticeMessageContent = "There will be a design review meeting on Wednesday at 2 PM to ensure consistency and quality in our design work.",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.Team
                },
                new Notice
                {
                    TeamId = 2,
                    OwnerID = managerID,
                    NoticeTitle = "Creative Inspiration Workshop",
                    NoticeMessageContent = "Join us for a creative inspiration workshop next week to explore new design trends and brainstorm innovative ideas.",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.Team
                },
                new Notice
                {
                    OwnerID = vanahID,
                    NoticeTitle = "Employee Recognition Awards",
                    NoticeMessageContent = "It's time to nominate outstanding colleagues for our Employee Recognition Awards. Let's celebrate and appreciate our team members.",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.Company
                },
                new Notice
                {
                    OwnerID = managerID,
                    NoticeTitle = "Company Quarterly Update",
                    NoticeMessageContent = "Join us for our quarterly company-wide update meeting on [Date] to get insights on our progress and upcoming initiatives.",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.Company
                },
                new Notice
                {
                    OwnerID = managerID,
                    NoticeTitle = "Upcoming Holiday Office Closure",
                    NoticeMessageContent = "Our offices will be closed on the 25th of December for Christmas. Please plan your work accordingly, and enjoy the holiday!",
                    DateCreated = DateTime.Now,
                    PublicityStatus = PublicityStatus.Company
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

            CreateNotificationGroups(context);
            CreateEmployeeNotificationGroups(context);

            var notifications = new List<Notification>();
            var jn = new JobNotification();
            jn.SetJobNotification(jobs[2]);
            var nn = new NoticeNotification();
            var companyNG = context.NotificationGroups
                .FirstOrDefault(ng => ng.GroupName == "Company");
            nn.SetNoticeNotification(notices[6], companyNG.Id);
            notifications.Add(jn);
            notifications.Add(nn);

            context.Notifications.AddRange(notifications);
            context.SaveChanges();
        }

        private static void CreateNotificationGroups(ApplicationDbContext context)
        {
            var projects = context.Projects.ToList();
            var teams = context.Teams.ToList();
            var notificationGroups = new List<NotificationGroup>();

            foreach (var  project in projects)
            {
                var notificationGroup = new NotificationGroup
                {
                    GroupName = project.ProjectName
                };

                notificationGroups.Add(notificationGroup);
            }

            foreach (var team in teams)
            {
                var notificationGroup = new NotificationGroup
                {
                    GroupName = team.TeamName
                };

                notificationGroups.Add(notificationGroup);
            }

            notificationGroups.Add(new NotificationGroup { GroupName = "Company" });

            context.NotificationGroups.AddRange(notificationGroups); 
            context.SaveChanges();
        }

        private static void CreateEmployeeNotificationGroups(ApplicationDbContext context)
        {
            var employees = context.Employees.ToList();

            List<NotificationGroup> notificationGroups = new List<NotificationGroup>();
            List<EmployeeNotificationGroup> employeeNotificationGroups = new List<EmployeeNotificationGroup>();

            // Add every employee to Company notification group
            var companyNG = context.NotificationGroups
                .FirstOrDefault(ng => ng.GroupName == "Company");
            if (companyNG != null)
            {
                var companyNgId = companyNG.Id;

                foreach (var employee in employees)
                {
                    var notificationGroup = new EmployeeNotificationGroup
                    {
                        EmployeeId = employee.Id,
                        NotificationGroupId = companyNgId
                    };

                    if (!context.EmployeeNotificationGroups
                    .Any(eng => eng.EmployeeId == notificationGroup.EmployeeId && eng.NotificationGroupId == notificationGroup.NotificationGroupId))
                    {
                        employeeNotificationGroups.Add(notificationGroup);
                    }
                }                
            }

            // Add every employee to the notification groups for their teams
            var employeeTeams = context.EmployeeTeams
                .Include(et => et.Team)
                .Include(et => et.Employee)
                .ToList();

            foreach (var et in employeeTeams)
            {
                var notificationGroup = context.NotificationGroups.FirstOrDefault(ng => ng.GroupName == et.Team.TeamName);

                if (notificationGroup != null)
                {
                    var employeeNotificationGroup = new EmployeeNotificationGroup
                    {
                        EmployeeId = et.EmployeeId,
                        NotificationGroupId = notificationGroup.Id
                    };

                    employeeNotificationGroups.Add(employeeNotificationGroup);
                }
            }

            // Add every employee to the notification group for their projects
            var employeeProjects = context.EmployeesProjects
                .Include(ep => ep.Project)
                .Include(ep => ep.Employee)
                .ToList();

            foreach (var ep in employeeProjects)
            {
                var notificationGroup = context.NotificationGroups.FirstOrDefault(ng => ng.GroupName == ep.Project.ProjectName);

                if (notificationGroup != null)
                {
                    var employeeNotificationGroup = new EmployeeNotificationGroup
                    {
                        EmployeeId = ep.EmployeeId,
                        NotificationGroupId = notificationGroup.Id
                    };

                    employeeNotificationGroups.Add(employeeNotificationGroup);
                }
            }

            context.EmployeeNotificationGroups.AddRange(employeeNotificationGroups);
            context.SaveChanges();
        }
    }
}
