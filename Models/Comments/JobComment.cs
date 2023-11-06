using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Models.Comments
{
    public class JobComment : IComment
    {
        [Key]
        public int CommentId { get; set; }
        public int ParentId { get; set; }
        public string OwnerID { get; set; }
        public Employee Owner { get; set; }
        public string CommentText { get; set; }
        public DateTime CreationDate { get; set; }

        public Job Job { get; set; }
    }
}
