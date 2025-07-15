using AutoMapper;
using HandInHand.Data;
using HandInHand.Dtos;
using HandInHand.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt; 

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
        // 1. 从 JWT 中解析当前登录用户 Id
        var claim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst(JwtRegisteredClaimNames.Sub);
        if (claim is null) return Unauthorized();

        var userId = int.Parse(claim.Value);

        // 2. 将 DTO 映射到实体并强制写入 RequesterId/CreatedAt
        var entity = mapper.Map<HelpRequest>(dto);
        entity.RequesterId = userId;
        entity.CreatedAt = DateTime.UtcNow;
        db.HelpRequests.Add(entity);
        await db.SaveChangesAsync();

        var resultDto = mapper.Map<HelpRequestDto>(entity);
        return CreatedAtAction(nameof(GetHelpRequest), new { id = entity.Id },
                               resultDto);
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
