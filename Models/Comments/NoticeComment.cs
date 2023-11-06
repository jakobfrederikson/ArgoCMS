using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Models.Comments
{
    public class NoticeComment : IComment
    {
        [Key]
        public int CommentId { get; set; }
        public int ParentId { get; set; }
        public string OwnerID { get; set; }
        public Employee Owner { get; set; }
        public string CommentText { get; set; }
        public DateTime CreationDate { get; set; }

        public Notice Notice { get; set; }
    }
}
