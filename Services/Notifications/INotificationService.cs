using ArgoCMS.Data;
using ArgoCMS.Models.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace ArgoCMS.Services.Notifications
{
	public interface INotificationService
	{
		List<Notification> GetAllUnread();
	}
}
