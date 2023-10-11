using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Models
{
    public class Notice
    {
        public int NoticeId { get; set; }

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
    }
}
