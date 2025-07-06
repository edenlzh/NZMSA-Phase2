using HandInHand.Data;
using HandInHand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandInHand.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(AppDbContext db) : ControllerBase
{
    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        => await db.Users
                   .AsNoTracking()
                   .Include(u => u.Skills)
                   .ToListAsync();

    // GET: api/users/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await db.Users
                           .AsNoTracking()
                           .Include(u => u.Skills)
                           .FirstOrDefaultAsync(u => u.Id == id);

        return user is null ? NotFound() : user;
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // PUT: api/users/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        if (id != user.Id) return BadRequest("ID 不一致");
        db.Entry(user).State = EntityState.Modified;

        try { await db.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException) when (!UserExists(id))
        { return NotFound(); }

        return NoContent();
    }

    // DELETE: api/users/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await db.Users.FindAsync(id);
        if (user is null) return NotFound();

        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private bool UserExists(int id) => db.Users.Any(e => e.Id == id);
}
