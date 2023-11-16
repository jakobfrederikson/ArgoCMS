using ArgoCMS.Authorization.Employees;
using ArgoCMS.Data;
using ArgoCMS.Models;
using ArgoCMS.Hubs;
using ArgoCMS.Services.Notifications;
using ArgoCMS.Services.Jobs;
using ArgoCMS.Services.Dashboard;
using ArgoCMS.Authorization.Projects;
using ArgoCMS.Authorization.Teams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var azureSQLConnectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(azureSQLConnectionString));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["AZURE_REDIS_CONNECTIONSTRING"];
    options.InstanceName = "SampleInstance";
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Logging.AddAzureWebAppDiagnostics();

builder.Services.AddDefaultIdentity<Employee>(
    options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddSignalR();

builder.Services.AddHttpContextAccessor();

// Require authorization unless specified.
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});


// Require admin access for any pages in the admin folder.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Administrators"));
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin", "AdminPolicy");
});

// Authorization handlers.
// Projects
builder.Services.AddScoped<IAuthorizationHandler,
                        ProjectIsOwnerAuthorizationHandler>();

builder.Services.AddSingleton<IAuthorizationHandler,
                        ProjectAdministratorAuthorizationHandler>();

//Teams
builder.Services.AddScoped<IAuthorizationHandler,
                        TeamIsOwnerAuthorizationHandler>();

builder.Services.AddSingleton<IAuthorizationHandler,
                        TeamAdministratorAuthorizationHandler>();
// Employees
builder.Services.AddScoped<IAuthorizationHandler,
                    EmployeeIsOwnerAuthorizationHandler>();

builder.Services.AddSingleton<IAuthorizationHandler,
                    EmployeeAdministratorAuthorizationHandler>();

// Services (Add Scoped because they use EF)
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    //context.Database.Migrate();
    // requires using Microsoft.Extensions.Configuration;
    // Set password with the Secret Manager tool.
    // dotnet user-secrets set SeedUserPW <pw>

    var testUserPw = "Passw0rd!";

    await SeedData.Initialize(services, testUserPw);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Map("/api/Notifications", (INotificationService service) =>
{
    try
    {
        var notifications = service.GetAll();
        return Results.Ok(notifications);
    }
    catch (Exception ex)
    {
		return Results.Problem(ex.Message);
	}
});

app.Map("/api/Notifications/DeleteNotification/{objectId:int}", (int objectId, INotificationService service) =>
{
    try
    {
        service.DeleteNotification(objectId);
        return Results.Ok($"Deleted notification: {objectId}");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Map("/api/Notifications/MarkNotificationAsRead/{objectId:int}", (int objectId, INotificationService service) =>
{
    try
    {
        service.MarkNotificationAsRead(objectId);
        return Results.Ok($"Notification marked as read: {objectId}");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Map("/api/Jobs/GetEmployeesByTeam/{teamId:int}", (int teamId, IJobService service) =>
{
    try
    {
        var employees = service.GetEmployeesByTeam(teamId);
        return Results.Ok(employees);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Map("/api/Dashboard/LoadListOfColours/{numberOfEmployees:int}", (int numberOfEmployees, IDashboardService service) =>
{
    try
    {
        var employees = service.ListOfColours(numberOfEmployees);
        return Results.Ok(employees);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Map("/api/Dashboard/LoadTeamJobStats/{teamId:int}", (int teamId, IDashboardService service) =>
{
    try
    {
        var employees = service.GetTeamJobStatistics(teamId);
        return Results.Ok(employees);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
