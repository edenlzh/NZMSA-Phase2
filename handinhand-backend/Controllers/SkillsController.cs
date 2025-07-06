using HandInHand.Data;
using HandInHand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandInHand.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillsController(AppDbContext db) : ControllerBase
{
    // 可通过 ?userId=1 过滤某用户技能
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Skill>>> GetSkills([FromQuery] int? userId = null)
    {
        var query = db.Skills.AsNoTracking();
        if (userId is not null) query = query.Where(s => s.UserId == userId);
        return await query.Include(s => s.User).ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Skill>> GetSkill(int id)
    {
        var skill = await db.Skills
                            .AsNoTracking()
                            .Include(s => s.User)
                            .FirstOrDefaultAsync(s => s.Id == id);

        return skill is null ? NotFound() : skill;
    }

    [HttpPost]
    public async Task<ActionResult<Skill>> PostSkill(Skill skill)
    {
        // 确认外键有效
        if (!db.Users.Any(u => u.Id == skill.UserId))
            return BadRequest($"User {skill.UserId} 不存在");

        db.Skills.Add(skill);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSkill), new { id = skill.Id }, skill);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutSkill(int id, Skill skill)
    {
        if (id != skill.Id) return BadRequest("ID 不一致");
        db.Entry(skill).State = EntityState.Modified;

        try { await db.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException) when (!SkillExists(id))
        { return NotFound(); }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSkill(int id)
    {
        var skill = await db.Skills.FindAsync(id);
        if (skill is null) return NotFound();

        db.Skills.Remove(skill);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private bool SkillExists(int id) => db.Skills.Any(e => e.Id == id);
}
