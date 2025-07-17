using AutoMapper;
using HandInHand.Data;
using HandInHand.Dtos;
using HandInHand.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HandInHand.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController(AppDbContext db, IMapper mapper) : ControllerBase
{
    /* ---------- 查询 ---------- */
    [HttpGet]
    public async Task<IEnumerable<CommentDto>> List(
        [FromQuery] int? skillId,
        [FromQuery(Name = "helpRequestId")] int? reqId,
        [FromQuery] int page = 1)
    {
        if (skillId is null && reqId is null)
            return Enumerable.Empty<CommentDto>();

        var q = db.Comments
                  .Include(c => c.Author)
                  .AsQueryable();

        if (skillId is not null) q = q.Where(c => c.SkillId == skillId);
        if (reqId  is not null) q = q.Where(c => c.HelpRequestId == reqId);

        var comments = await q.OrderByDescending(c => c.Id)
                              .Skip((page - 1) * 6)
                              .Take(6)
                              .ToListAsync();

        return mapper.Map<List<CommentDto>>(comments);
    }

    /* ---------- 新建 ---------- */
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromQuery] int? skillId,
        [FromQuery(Name = "helpRequestId")] int? reqId,
        CommentCreateDto dto)
    {
        if (skillId is null && reqId is null)
            return BadRequest("skillId or helpRequestId required");

        var comment = mapper.Map<Comment>(dto);
        comment.SkillId        = skillId;
        comment.HelpRequestId  = reqId;
        comment.AuthorId       = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        db.Comments.Add(comment);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(List), new { id = comment.Id }, mapper.Map<CommentDto>(comment));
    }
}
