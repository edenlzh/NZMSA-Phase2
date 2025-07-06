namespace HandInHand.Models;
public class User {
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    // Tailwind 等前端可根据此 URL 拉头像，留空则用默认图
    public string? AvatarUrl { get; set; }
    // 导航属性：用户拥有的技能
    public List<Skill>? Skills { get; set; }
}