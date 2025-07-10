using AutoMapper;
using HandInHand.Data;
using HandInHand.Dtos;
using HandInHand.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandInHand.Controllers;

[Authorize]                                // 全局需要登录
[ApiController]
[Route("api/[controller]")]
public class UsersController(AppDbContext db, IMapper mapper) : ControllerBase
{
    // 允许匿名查看用户列表（例：注册前浏览）
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await db.Users.Include(u => u.Skills)
                                  .AsNoTracking()
                                  .ToListAsync();
        return mapper.Map<List<UserDto>>(users);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await db.Users.Include(u => u.Skills)
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(u => u.Id == id);
        return user is null ? NotFound() : mapper.Map<UserDto>(user);
    }

    [HttpPost]                              // 需要登录
    public async Task<ActionResult<UserDto>> PostUser(UserDto dto)
    {
        var entity = mapper.Map<User>(dto);
        db.Users.Add(entity);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = entity.Id },
                               mapper.Map<UserDto>(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutUser(int id, UserDto dto)
    {
        if (id != dto.Id) return BadRequest("ID 不一致");

        var entity = await db.Users.FindAsync(id);
        if (entity is null) return NotFound();

        mapper.Map(dto, entity);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var entity = await db.Users.FindAsync(id);
        if (entity is null) return NotFound();

        db.Users.Remove(entity);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
