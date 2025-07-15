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
        // 从 JWT 里取当前登录用户 Id（在 TokenService 里我们把 Id 存在 sub）
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst(JwtRegisteredClaimNames.Sub);
        if (userIdClaim is null) return Unauthorized();

        var userId = int.Parse(userIdClaim.Value);

        var entity = mapper.Map<Skill>(dto);
        entity.UserId = userId; // 强制绑定当前用户
        db.Skills.Add(entity);
        await db.SaveChangesAsync();

        var resultDto = mapper.Map<SkillDto>(entity);
        return CreatedAtAction(nameof(GetSkill), new { id = entity.Id },
                               resultDto);
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
