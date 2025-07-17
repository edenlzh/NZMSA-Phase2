using HandInHand.Models;
using Microsoft.EntityFrameworkCore;

namespace HandInHand.Data;
public class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<HelpRequest> HelpRequests => Set<HelpRequest>();
    public DbSet<Comment> Comments => Set<Comment>();

    // 演示：创建复合/默认值或约束时可重写 OnModelCreating
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 用户名唯一索引
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        // 建立用户与技能的一对多关系
        modelBuilder.Entity<Skill>()
            .HasOne(s => s.User)
            .WithMany(u => u.Skills)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Skill>()
            .HasMany(s => s.Comments)
            .WithOne(c => c.Skill!)
            .HasForeignKey(c => c.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        // 建立帮助请求与用户的一对多关系
        modelBuilder.Entity<HelpRequest>()
            .HasOne(h => h.Requester)
            .WithMany(u => u.HelpRequests)
            .HasForeignKey(h => h.RequesterId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HelpRequest>()
            .HasMany(h => h.Comments)
            .WithOne(c => c.HelpRequest!)
            .HasForeignKey(c => c.HelpRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        // 建立评论与用户的一对多关系
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Parent)
            .WithMany()
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}