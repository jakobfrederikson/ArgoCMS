namespace ArgoCMS.Services.Dashboard
{
    public interface IDashboardService
    {
        List<string> ListOfColours(int numberOfEmployees);
        Dictionary<string, int> GetTeamJobStatistics(int teamId);
    }
}
