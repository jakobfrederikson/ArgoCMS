namespace ArgoCMS.Services.Dashboard
{
    public interface IDashboardService
    {
        List<string> ListOfColours(int numberOfEmployees);
        Task<Dictionary<string, int>> GetTeamJobStatisticsAsync(int teamId);
    }
}
