namespace ArgoCMS.Models.JointEntities
{
	public class TeamProject
	{
		public int TeamId { get; set; }
		public Team Team { get; set; }

		public int ProjectId { get; set; }
		public Project Project { get; set; }
	}
}
