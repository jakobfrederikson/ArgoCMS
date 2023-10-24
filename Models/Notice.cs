using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Models
{
    public class Notice
    {
        public int NoticeId { get; set; }

        [Display(Name = "Team")]
        public int TeamId { get; set; }

        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        [Required]
        [Display(Name = "Created by")]
        public string OwnerID { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string NoticeTitle { get; set; }

        [Required]
        [Display(Name = "Message content")]
        [DataType(DataType.MultilineText)]
        public string NoticeMessageContent { get; set; }

        [Required]
        [Display(Name = "Date created")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        [Required]
        [Display(Name = "Publicity Status")]
        public PublicityStatus PublicityStatus { get; set; }
    }

    public enum PublicityStatus
    {
        OnlyTeam,
        OnlyProject,
        OnlyRole,
        Everyone
    }
}
