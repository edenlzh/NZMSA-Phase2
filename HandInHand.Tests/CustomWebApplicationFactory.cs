using HandInHand.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace HandInHand.Tests;

/// <summary>
/// 自定义测试服务器，使用 In-Memory EFCore 与测试版 JWT 设置
/// </summary>
public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        // 注入测试配置
        builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            cfg.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = "DataSource=:memory:"
            });
        });

        builder.ConfigureServices(services =>
        {
            // 替换真实数据库
            var descriptor = services.Single(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(opt =>
                opt.UseInMemoryDatabase("HandInHand_TestDb"));

            // 确保 In-Memory DB 建立
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();

            var env = sp.GetRequiredService<IWebHostEnvironment>();
            env.WebRootPath = Path.Combine(Path.GetTempPath(), "wwwroot");
            Directory.CreateDirectory(env.WebRootPath);
        });
    }
}
