using ArgoCMS.Data;
using ArgoCMS.Models;
using ArgoCMS.Models.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ArgoCMS.Services.Notifications
{
    public class NotificationService : INotificationService
    {

        private readonly IServiceScopeFactory _scopeFactory;

        public NotificationService(IServiceScopeFactory scopeFactory)
        {
            this._scopeFactory = scopeFactory;
        }

        public List<Notification> GetAllUnread()
        {

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var test = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

            var currentEmployee = test.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var listOfNotifications = context.Notifications
                .Where(n => n.UserId == currentEmployee && n.IsRead == false)
                .ToList();

            return listOfNotifications;
        }
    }
}
