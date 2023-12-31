﻿using System.ComponentModel.DataAnnotations;
using ArgoCMS.Models.Comments;

namespace ArgoCMS.Models
{
    public class Notice
    {
        public int NoticeId { get; set; }

        [Display(Name = "Team")]
        public int? TeamId { get; set; }
        public Team? Team { get; set; }

        [Display(Name = "Project")]
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }

        [Required]
        [Display(Name = "Created by")]
        public string OwnerID { get; set; }
        public Employee Owner { get; set; }

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

        public List<NoticeComment> Comments { get; set; } // Navigation property
    }

    public enum PublicityStatus
    {
        Company,
        Project,
        Team
    }
}
