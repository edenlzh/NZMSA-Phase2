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
public class HelpRequestsController(AppDbContext db, IMapper mapper) : ControllerBase
{
    /* ---------- 匿名查看 ---------- */
    [HttpGet]
    public async Task<IEnumerable<HelpRequestDto>> List([FromQuery] int? userId)
    {
        var q = db.HelpRequests.Include(r => r.Requester).AsQueryable();
        if (userId != null) q = q.Where(r => r.RequesterId == userId);
        return mapper.Map<List<HelpRequestDto>>(await q.ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HelpRequestDto>> Detail(int id)
    {
        var r = await db.HelpRequests.Include(x => x.Requester).FirstOrDefaultAsync(x => x.Id == id);
        return r is null ? NotFound() : mapper.Map<HelpRequestDto>(r);
    }

    /* ---------- 创建 ---------- */
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<HelpRequestDto>> Create(HelpRequestDto dto)
    {
        var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var entity = mapper.Map<HelpRequest>(dto);
        entity.RequesterId = me;
        db.HelpRequests.Add(entity);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Detail), new { id = entity.Id }, mapper.Map<HelpRequestDto>(entity));
    }

    /* ---------- 我的求助 ---------- */
    [Authorize]
    [HttpGet("my")]          // 新增
    [HttpGet("me")]          // 兼容旧路径
    public async Task<IEnumerable<HelpRequestDto>> My()
    {
        var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var list = await db.HelpRequests
            .Where(r => r.RequesterId == me)
            .Include(r => r.Requester)
            .ToListAsync();
        return mapper.Map<List<HelpRequestDto>>(list);   // 允许返回空集合
    }

    /* ---------- 更新 ---------- */
    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, HelpRequestDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var entity = await db.HelpRequests.FindAsync(id);
        if (entity is null) return NotFound();

        var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (entity.RequesterId != me) return Forbid();

        mapper.Map(dto, entity);
        await db.SaveChangesAsync();
        return Ok(new { message = "Request updated" });
    }

    /* ---------- 删除 ---------- */
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await db.HelpRequests.Include(r => r.Comments).FirstOrDefaultAsync(r => r.Id == id);
        if (entity is null) return NotFound();

        var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (entity.RequesterId != me) return Forbid();

        db.HelpRequests.Remove(entity);   // 级联删除评论
        await db.SaveChangesAsync();
        return Ok(new { message = "Request deleted" });
    }
}
