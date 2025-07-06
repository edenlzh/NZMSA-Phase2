namespace HandInHand.Models;
public class HelpRequest {
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    public static DateTime GetNewZealandTimeNow()
    {
        var nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, nzTimeZone);
    }
    public DateTime CreatedAt { get; set; } = GetNewZealandTimeNow();

    // 请求者
    public int RequesterId { get; set; }
    public User? Requester { get; set; }

    // 是否已解决
    public bool IsResolved { get; set; } = false;
}