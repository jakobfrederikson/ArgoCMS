using ArgoCMS.Data;
using ArgoCMS.Hubs;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace ArgoCMS.Pages
{
    public class DependencyInjection_Hub_BasePageModel : PageModel
    {
        protected ApplicationDbContext Context { get; }
        protected IAuthorizationService AuthorizationService { get; }
        protected UserManager<Employee> UserManager { get; }
        protected IHubContext<NotificationHub> HubContext { get; }

        public DependencyInjection_Hub_BasePageModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<Employee> userManager,
            IHubContext<NotificationHub> hubContext) : base()
        {
            Context = context;
            AuthorizationService = authorizationService;
            UserManager = userManager;
            HubContext = hubContext;
        }
    }
}
