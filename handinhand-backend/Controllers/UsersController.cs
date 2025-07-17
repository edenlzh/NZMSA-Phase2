using AutoMapper;
using HandInHand.Data;
using HandInHand.Dtos;
using HandInHand.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HandInHand.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController(
        AppDbContext db,
        IMapper mapper,
        IWebHostEnvironment env,
        IPasswordHasher<User> hasher)
    : ControllerBase
{
    /* ---------- 公共列表 ---------- */
    [AllowAnonymous]
    [HttpGet]
    public async Task<IEnumerable<UserDto>> GetUsers()
        => mapper.Map<List<UserDto>>(await db.Users.ToListAsync());

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
        => mapper.Map<UserDto>(await db.Users.FindAsync(id));

    /* ---------- 当前用户资料 ---------- */
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> Me()
        => mapper.Map<UserDto>(await Current());

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe(UserUpdateDto dto)
    {
        var user = await Current();

        /* 用户名 / 邮箱唯一校验 */
        if (!string.IsNullOrWhiteSpace(dto.UserName) &&
            dto.UserName != user.UserName)
        {
            if (await db.Users.AnyAsync(u => u.UserName == dto.UserName))
                return Conflict("UserName taken");
            user.UserName = dto.UserName;
        }
        if (!string.IsNullOrWhiteSpace(dto.Email) &&
            dto.Email != user.Email)
        {
            if (await db.Users.AnyAsync(u => u.Email == dto.Email))
                return Conflict("Email taken");
            user.Email = dto.Email;
        }

        /* 修改密码 */
        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            if (string.IsNullOrWhiteSpace(dto.OldPassword) ||
                hasher.VerifyHashedPassword(user, user.PasswordHash, dto.OldPassword)
                    != PasswordVerificationResult.Success)
                return BadRequest("Old password incorrect");

            user.PasswordHash = hasher.HashPassword(user, dto.NewPassword);
        }

        /* 头像 URL */
        if (!string.IsNullOrWhiteSpace(dto.AvatarUrl))
            user.AvatarUrl = dto.AvatarUrl;

        await db.SaveChangesAsync();
        return NoContent();
    }

    /* ---------- 上传头像 ---------- */
    [HttpPost("me/avatar")]
    public async Task<ActionResult<string>> UploadAvatar(IFormFile file)
    {
        if (file.Length == 0) return BadRequest("Empty file");

        var folder = Path.Combine(env.WebRootPath, "avatars");
        Directory.CreateDirectory(folder);
        var ext = Path.GetExtension(file.FileName);
        var name = $"{Guid.NewGuid()}{ext}";
        var path = Path.Combine(folder, name);

        await using var fs = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(fs);

        var url = $"/avatars/{name}";
        var user = await Current();
        user.AvatarUrl = url;
        await db.SaveChangesAsync();

        return Created(user.AvatarUrl, user.AvatarUrl);
    }

    /* ---------- 删除账号 ---------- */
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMe()
    {
        var user = await Current();

        /* EF 级联：删除用户 ⇒ 其 Skills / HelpRequests / Comments 均删除 */
        db.Users.Remove(user);
        await db.SaveChangesAsync();

        return Ok(new { message = "Account deleted" });   // 便于前端 toast
    }

    /* ---------- 内部帮助 ---------- */
    private async Task<User> Current()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(idClaim, out var id))
            throw new ApplicationException("Invalid user id in JWT");

        return await db.Users.FirstAsync(u => u.Id == id);
    }
}
