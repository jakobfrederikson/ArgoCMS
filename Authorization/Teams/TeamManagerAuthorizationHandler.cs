using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using ArgoCMS.Models;

namespace ArgoCMS.Authorization.Teams
{
    public class TeamManagerAuthorizationHandler
    : AuthorizationHandler<OperationAuthorizationRequirement, Team>
    {
        protected override Task HandleRequirementAsync(
                                              AuthorizationHandlerContext context,
                                              OperationAuthorizationRequirement requirement,
                                              Team resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }
 
            // If asking to delete or create a team, return.
            if (requirement.Name == Constants.CreateOperationName ||
                requirement.Name == Constants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }


            // Administrators can do anything.
            if (context.User.IsInRole(Constants.ManagersRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
