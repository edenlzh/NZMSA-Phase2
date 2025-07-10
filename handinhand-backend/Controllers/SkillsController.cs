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
public class SkillsController(AppDbContext db, IMapper mapper) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SkillDto>>> GetSkills([FromQuery] int? userId = null)
    {
        var q = db.Skills.Include(s => s.User).AsNoTracking();
        if (userId is not null) q = q.Where(s => s.UserId == userId);
        var list = await q.ToListAsync();
        return mapper.Map<List<SkillDto>>(list);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SkillDto>> GetSkill(int id)
    {
        var entity = await db.Skills.Include(s => s.User)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(s => s.Id == id);
        return entity is null ? NotFound() : mapper.Map<SkillDto>(entity);
    }

    [HttpPost]
    public async Task<ActionResult<SkillDto>> PostSkill(SkillDto dto)
    {
        if (!db.Users.Any(u => u.Id == dto.UserId))
            return BadRequest($"User {dto.UserId} 不存在");

        var entity = mapper.Map<Skill>(dto);
        db.Skills.Add(entity);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSkill), new { id = entity.Id },
                               mapper.Map<SkillDto>(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutSkill(int id, SkillDto dto)
    {
        if (id != dto.Id) return BadRequest();

        var entity = await db.Skills.FindAsync(id);
        if (entity is null) return NotFound();

        mapper.Map(dto, entity);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSkill(int id)
    {
        var entity = await db.Skills.FindAsync(id);
        if (entity is null) return NotFound();

        db.Skills.Remove(entity);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
