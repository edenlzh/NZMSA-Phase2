namespace HandInHand.Dtos;

public class CommentCreateDto
{
    public string Content    { get; set; } = string.Empty;

    // 若做楼中楼，可带父级；没有就留 null
    public int?   ParentId   { get; set; }
}
