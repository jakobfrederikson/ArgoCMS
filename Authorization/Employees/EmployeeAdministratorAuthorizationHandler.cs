using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using ArgoCMS.Models;

namespace ArgoCMS.Authorization.Employees
{
    public class EmployeeAdministratorAuthorizationHandler
    : AuthorizationHandler<OperationAuthorizationRequirement, Employee>
    {
        protected override Task HandleRequirementAsync(
                                              AuthorizationHandlerContext context,
                                              OperationAuthorizationRequirement requirement,
                                              Employee resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            // Administrators can do anything.
            if (context.User.IsInRole(Constants.AdministratorsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
