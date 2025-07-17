namespace HandInHand.Models;
public class Comment {
    public int Id { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int AuthorId  { get; set; }
    public User? Author  { get; set; }

    public int? SkillId        { get; set; }
    public Skill? Skill        { get; set; }
    public int? HelpRequestId  { get; set; }
    public HelpRequest? HelpRequest { get; set; }

    public int? ParentId { get; set; }        // 用于楼中楼回复
    public Comment? Parent { get; set; }
}
