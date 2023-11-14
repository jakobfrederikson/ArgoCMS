using ArgoCMS.Data;
using ArgoCMS.Models.Notifications;
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

        public List<Notification> GetAll()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var test = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

            var userId = test.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userNotifications = context.Notifications
                .Where(n => n.EmployeeId == userId)
                .ToList();

            var userGroups = context.EmployeeNotificationGroups
                .Where(eng => eng.EmployeeId == userId)
                .Select(eng => eng.NotificationGroup)
                .ToList();

            var userGroupsWithNotifications = context.NotificationGroups
                .Where(ng => userGroups.Contains(ng))
                .Include(ng => ng.NoticeNotifications)
                .ToList();

            var allUserNotifications = new List<Notification>();

            foreach (var item in userGroupsWithNotifications)
            {
                if (item.NoticeNotifications != null)
                {
                    allUserNotifications.AddRange(item.NoticeNotifications);
                }
            }

            allUserNotifications.AddRange(userNotifications);

            var sortedNotifications = allUserNotifications
                .OrderBy(n => n.IsRead)
                .ThenByDescending(n => n.TimeStamp)
                .ToList();

			return sortedNotifications;
		}

        public void DeleteNotification(int notificationId)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var notification = context.Notifications
                .FirstOrDefault(n => n.NotificationId == notificationId);

            if (notification != null)
            {
                context.Notifications.Remove(notification);
                context.SaveChanges();
            }
        }

        public void MarkNotificationAsRead(int notificationId)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var notification = context.Notifications
                .FirstOrDefault(n => n.NotificationId == notificationId);

            if (notification != null)
            {
                notification.IsRead = true;
                context.Update(notification);
                context.SaveChanges();
            }
        }
    }
}
