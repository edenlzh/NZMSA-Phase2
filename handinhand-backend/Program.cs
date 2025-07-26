using HandInHand.Data;
using HandInHand.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HandInHand.Services;
using Microsoft.AspNetCore.Identity;
using HandInHand.Models;

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

builder.Services.AddAutoMapper(typeof(Program));

// builder.Services.AddIdentityCore<User>()
//                 .AddRoles<IdentityRole>()
//                 .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllers();

// JWT
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer   = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<TokenService>();

// ---------------- 构建并配置管道 ----------------
var app = builder.Build();

app.UseStaticFiles();

// ★★★ 自动迁移：首次部署或有新迁移时自动创建 / 更新数据库 ★★★
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //只在关系型数据库上执行迁移
    if (db.Database.IsRelational())
    {
        db.Database.Migrate();               // 应用所有待执行的迁移
    }
    // 如需初始化种子数据，可在此处调用 DbSeeder.Seed(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors("client");

// （1）健康检查端点
app.MapGet("/", () => Results.Ok("HandInHand API running successfully!"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
