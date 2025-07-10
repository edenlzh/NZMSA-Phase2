namespace HandInHand.Dtos;

public class SkillDto
{
    public int    Id          { get; set; }
    public string Title       { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category   { get; set; }
    public int    UserId      { get; set; }
    public string? UserName   { get; set; }
}
