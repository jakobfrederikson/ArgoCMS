using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using ArgoCMS.Models;

namespace ArgoCMS.Authorization.Jobs
{
    public class JobAdministratorAuthorizationHandler
    : AuthorizationHandler<OperationAuthorizationRequirement, Job>
    {
        protected override Task HandleRequirementAsync(
                                              AuthorizationHandlerContext context,
                                              OperationAuthorizationRequirement requirement,
                                              Job resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            // Administrators can do anything.
            if (context.User.IsInRole(AuthorizationOperations.Constants.AdministratorsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
