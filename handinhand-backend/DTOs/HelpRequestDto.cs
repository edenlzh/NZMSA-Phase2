namespace HandInHand.Dtos;

public class HelpRequestDto
{
    public int      Id            { get; set; }
    public string   Title         { get; set; } = string.Empty;
    public string?  Description   { get; set; }
    public DateTime CreatedAt     { get; set; }
    public bool     IsResolved    { get; set; }
    public int      RequesterId   { get; set; }
    public string?  RequesterName { get; set; }
}
