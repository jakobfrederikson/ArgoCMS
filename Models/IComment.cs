namespace ArgoCMS.Models
{
    public interface IComment
    {
        int CommentId { get; set; }
        int ParentId { get; set; }
        string OwnerID { get; set; }
        string CommentText { get; set; }
        DateTime CreationDate { get; set; }
    }
}
