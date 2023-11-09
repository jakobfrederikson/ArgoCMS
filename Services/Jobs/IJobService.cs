using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgoCMS.Services.Jobs
{
    public interface IJobService
    {
        IQueryable<SelectListItem> GetEmployeesByTeam(int teamId);
    }
}
