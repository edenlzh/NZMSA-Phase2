namespace HandInHand.Models;
public class Skill {
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    // 简单打标签，前端可做筛选
    public string? Category { get; set; }
    // 外键
    public int UserId { get; set; }
    // 导航属性
    public User? User { get; set; }

    public ICollection<Comment> Comments { get; } = new List<Comment>();
}