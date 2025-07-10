using AutoMapper;
using HandInHand.Data;
using HandInHand.Dtos;
using HandInHand.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandInHand.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HelpRequestsController(AppDbContext db, IMapper mapper) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HelpRequestDto>>> GetHelpRequests(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (page <= 0 || pageSize <= 0) return BadRequest("分页参数错误");

        var q = db.HelpRequests.Include(h => h.Requester)
                               .OrderByDescending(h => h.CreatedAt)
                               .AsNoTracking();

        var total = await q.CountAsync();
        var data  = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        Response.Headers.Append("X-Total-Count", total.ToString());

        return mapper.Map<List<HelpRequestDto>>(data);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<HelpRequestDto>> GetHelpRequest(int id)
    {
        var entity = await db.HelpRequests.Include(h => h.Requester)
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(h => h.Id == id);
        return entity is null ? NotFound() : mapper.Map<HelpRequestDto>(entity);
    }

    [HttpPost]
    public async Task<ActionResult<HelpRequestDto>> PostHelpRequest(HelpRequestDto dto)
    {
        if (!db.Users.Any(u => u.Id == dto.RequesterId))
            return BadRequest($"User {dto.RequesterId} 不存在");

        var entity = mapper.Map<HelpRequest>(dto);
        db.HelpRequests.Add(entity);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetHelpRequest), new { id = entity.Id },
                               mapper.Map<HelpRequestDto>(entity));
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PatchHelpRequest(int id, [FromBody] bool isResolved)
    {
        var entity = await db.HelpRequests.FindAsync(id);
        if (entity is null) return NotFound();

        entity.IsResolved = isResolved;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteHelpRequest(int id)
    {
        var entity = await db.HelpRequests.FindAsync(id);
        if (entity is null) return NotFound();

        db.HelpRequests.Remove(entity);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
