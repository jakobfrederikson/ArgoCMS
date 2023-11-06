namespace ArgoCMS.Models.Comments
{
    public interface IComment
    {
        int CommentId { get; set; }
        int ParentId { get; set; }
        string OwnerID { get; set; }
        Employee Owner { get; set; }
        string CommentText { get; set; }
        DateTime CreationDate { get; set; }
    }
}
