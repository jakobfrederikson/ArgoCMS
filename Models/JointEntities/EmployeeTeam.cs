namespace ArgoCMS.Models.JointEntities
{
    public class EmployeeTeam
    {
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
