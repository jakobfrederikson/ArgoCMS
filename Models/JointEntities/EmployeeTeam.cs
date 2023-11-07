using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArgoCMS.Models.JointEntities
{
    public class EmployeeTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
