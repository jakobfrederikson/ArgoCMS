using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ArgoCMS.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        [Required]
        [Display(Name = "Notice parent")]
        public int NoticeId { get; set; }

        [Required]
        [Display(Name = "Commentor")]
        public string OwnerID { get; set; }

        [Required]
        [Display(Name = "Message content")]
        [DataType(DataType.MultilineText)]
        public string CommentText { get; set; }

        [Required]
        [Display(Name = "Time of post")]
        [DataType(DataType.DateTime)] 
        public DateTime CreationDate { get; set; }
    }
}
