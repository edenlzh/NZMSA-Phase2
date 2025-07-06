using HandInHand.Data;
using HandInHand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandInHand.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelpRequestsController(AppDbContext db) : ControllerBase
{
    // 支持分页：?page=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HelpRequest>>> GetHelpRequests(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (page <= 0 || pageSize <= 0) return BadRequest("分页参数错误");

        var query = db.HelpRequests
                      .AsNoTracking()
                      .Include(h => h.Requester)
                      .OrderByDescending(h => h.CreatedAt);

        var total = await query.CountAsync();
        var data  = await query.Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        // 加入响应头，方便前端显示总页数
        Response.Headers.Append("X-Total-Count", total.ToString());
        return data;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HelpRequest>> GetHelpRequest(int id)
    {
        var item = await db.HelpRequests
                           .AsNoTracking()
                           .Include(h => h.Requester)
                           .FirstOrDefaultAsync(h => h.Id == id);

        return item is null ? NotFound() : item;
    }

    [HttpPost]
    public async Task<ActionResult<HelpRequest>> PostHelpRequest(HelpRequest req)
    {
        if (!db.Users.Any(u => u.Id == req.RequesterId))
            return BadRequest($"User {req.RequesterId} 不存在");

        db.HelpRequests.Add(req);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetHelpRequest), new { id = req.Id }, req);
    }

    // 仅支持更新解决状态与描述
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PatchHelpRequest(int id, [FromBody] bool isResolved)
    {
        var req = await db.HelpRequests.FindAsync(id);
        if (req is null) return NotFound();

        req.IsResolved = isResolved;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteHelpRequest(int id)
    {
        var req = await db.HelpRequests.FindAsync(id);
        if (req is null) return NotFound();

        db.HelpRequests.Remove(req);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
