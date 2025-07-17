namespace HandInHand.Dtos;
public record CommentDto
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public required string AuthorName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ParentId { get; set; }
}
