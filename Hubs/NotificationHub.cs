using ArgoCMS.Data;
using ArgoCMS.Models.Notifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ApplicationDbContext _context;

        public NotificationHub(
            IHubContext<NotificationHub> hubContext
            , ApplicationDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        public override Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name;
            var user = _context.Employees
                .Include(e => e.NotificationGroups)
                .SingleOrDefault(u => u.UserName == userName);

            var connectionId = Context.ConnectionId;

            if (user != null)
            {
                if (user.NotificationGroups != null)
                {
                    foreach (var item in user.NotificationGroups)
                    {
                        Groups.AddToGroupAsync(connectionId, item.GroupName);
                    }
                }                
            }               

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userName = Context.User.Identity.Name;
            var user = _context.Employees
                .Include(e => e.NotificationGroups)
                .SingleOrDefault(u => u.UserName == userName);

            var connectionId = Context.ConnectionId;

            if (user != null)
            {
                if (user.NotificationGroups != null)
                {
                    foreach (var item in user.NotificationGroups)
                    {
                        Groups.RemoveFromGroupAsync(connectionId, item.GroupName);
                    }
                }                
            }

            return base.OnDisconnectedAsync(exception);
        }

        // Send notification to one user
        public async Task SendJobNotificationToUserAsync(string userId, Notification notification)
        {
            await Clients.User(userId).SendAsync("ReceiveJobNotification", notification);
        }

        // Send notification to a group
        public async Task SendNoticeNotificationToGroupAsync(string groupName, NoticeNotification notification)
        {
            await Clients.Group(groupName).SendAsync($"ReceiveNoticeNotification", notification);
        }

        public void UserJoinedTeam(string userId, string teamName)
        {
            var user = _context.Employees.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, teamName);
            }
        }

        public void UserLeftTeam(string userId, string teamName)
        {
            var user = _context.Employees.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                Groups.RemoveFromGroupAsync(Context.ConnectionId, teamName);
            }
        }

        public void UserJoinedProject(string userId, string projectName)
        {
            var user = _context.Employees.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, projectName);
            }
        }

        public void UserLeftProject(string userId, string projectName)
        {
            var user = _context.Employees.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                Groups.RemoveFromGroupAsync(Context.ConnectionId, projectName);
            }
        }
    }
}
