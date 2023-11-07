using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArgoCMS.Models.JointEntities
{
	public class TeamProject
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TeamId { get; set; }
		public Team Team { get; set; }

		public int ProjectId { get; set; }
		public Project Project { get; set; }
	}
}
