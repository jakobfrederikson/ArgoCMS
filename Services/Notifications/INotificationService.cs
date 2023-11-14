using ArgoCMS.Data;
using ArgoCMS.Models.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace ArgoCMS.Services.Notifications
{
	public interface INotificationService
	{
		List<Notification> GetAll();
		void DeleteNotification(int objectId);
		void MarkNotificationAsRead(int objectId);
    }
}
