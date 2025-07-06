using HandInHand.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------- 服务注册 ----------------
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// 自动生成 Swagger 文档，便于调试
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 允许前端在本地或 Azure 调用
builder.Services.AddCors(opt =>
    opt.AddPolicy("client",
        p => p.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin()));

builder.Services.AddControllers();

// ---------------- 构建并配置管道 ----------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("client");

// （1）健康检查端点
app.MapGet("/", () => Results.Ok("HandInHand API running 🚀"));

// （2）最小 API – Skills 示例，其余实体放到专用控制器中
app.MapGet("/api/skills", async (AppDbContext db) =>
    await db.Skills.AsNoTracking().ToListAsync());

app.MapPost("/api/skills", async (AppDbContext db, HandInHand.Models.Skill skill) =>
{
    db.Skills.Add(skill);
    await db.SaveChangesAsync();
    return Results.Created($"/api/skills/{skill.Id}", skill);
});

app.MapControllers();

app.Run();
