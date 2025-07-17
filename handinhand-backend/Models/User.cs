namespace HandInHand.Models;

using Microsoft.AspNetCore.Identity;

public class User {
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    // Tailwind 等前端可根据此 URL 拉头像，留空则用默认图
    public string AvatarUrl { get; set; } = "";
    // 导航属性：用户拥有的技能和求助请求
    public List<Skill>? Skills { get; set; }
    public List<HelpRequest>? HelpRequests { get; set; }
    public required string PasswordHash { get; set; }

    public void SetPassword(string raw, IPasswordHasher<User> hasher)
    {
        // 使用 ASP.NET Core Identity 的密码哈希器
        PasswordHash = hasher.HashPassword(this, raw);
    }
    public bool VerifyPassword(string raw, IPasswordHasher<User> hasher) => hasher.VerifyHashedPassword(this, PasswordHash, raw) == PasswordVerificationResult.Success;
}