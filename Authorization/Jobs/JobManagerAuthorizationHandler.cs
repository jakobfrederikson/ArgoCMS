using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using ArgoCMS.Models;

namespace ArgoCMS.Authorization.Jobs
{
    public class JobManagerAuthorizationHandler :
    AuthorizationHandler<OperationAuthorizationRequirement, Job>
    {
        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   Job resource)
    {
        if (context.User == null || resource == null)
        {
            return Task.CompletedTask;
        }

        // If not asking for approval/reject, return.
        if (requirement.Name != AuthorizationOperations.Constants.ApproveOperationName &&
            requirement.Name != AuthorizationOperations.Constants.RejectOperationName)
        {
            return Task.CompletedTask;
        }

        // Managers can approve or reject.
        if (context.User.IsInRole(AuthorizationOperations.Constants.ManagersRole))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
}
