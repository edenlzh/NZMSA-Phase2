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
public class SkillsController(AppDbContext db, IMapper mapper) : ControllerBase
{
    /* ---------- 匿名查看 ---------- */
    [HttpGet]
    public async Task<IEnumerable<SkillDto>> List([FromQuery] int? userId)
    {
        var query = db.Skills.Include(s => s.User).AsQueryable();
        if (userId != null) query = query.Where(s => s.UserId == userId);
        return mapper.Map<List<SkillDto>>(await query.ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SkillDto>> Detail(int id)
    {
        var s = await db.Skills.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        return s is null ? NotFound() : mapper.Map<SkillDto>(s);
    }

    /* ---------- 创建 ---------- */
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<SkillDto>> Create(SkillDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var entity = mapper.Map<Skill>(dto);
        entity.UserId = userId;
        db.Skills.Add(entity);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Detail), new { id = entity.Id }, mapper.Map<SkillDto>(entity));
    }

    /* ---------- 我的技能列表 ---------- */
    [Authorize]
    [HttpGet("my")]          // 配合测试用例
    [HttpGet("me")]          // 向后兼容原路径
    public async Task<IEnumerable<SkillDto>> My()
    {
        var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var list = await db.Skills
            .Where(s => s.UserId == me)
            .Include(s => s.User)
            .ToListAsync();
        return mapper.Map<List<SkillDto>>(list);   // 空集合也返回 200
    }

    /* ---------- 更新 ---------- */
    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, SkillDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var entity = await db.Skills.FindAsync(id);
        if (entity is null) return NotFound();

        var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (entity.UserId != me) return Forbid();

        mapper.Map(dto, entity);
        await db.SaveChangesAsync();
        return Ok(new { message = "Skill updated" });
    }

    /* ---------- 删除 ---------- */
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await db.Skills.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
        if (entity is null) return NotFound();

        var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (entity.UserId != me) return Forbid();

        db.Skills.Remove(entity);        // 级联删除评论
        await db.SaveChangesAsync();
        return Ok(new { message = "Skill deleted" });
    }
}
