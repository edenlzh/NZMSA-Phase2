using FluentAssertions;
using HandInHand.Dtos;
using HandInHand.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace HandInHand.Tests;

public class UsersPublicControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _c;
    public UsersPublicControllerTests(CustomWebApplicationFactory f) => _c = f.CreateClient();

    [Fact(DisplayName = "GET /api/users 列出所有用户")]
    public async Task List_Works()
    {
        var a = await _c.RegisterAndGetTokenAsync("u1", "u1@mail");
        var b = await _c.RegisterAndGetTokenAsync("u2", "u2@mail");

        var resp = await _c.GetAsync("/api/users");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var list = await resp.Content.ReadFromJsonAsync<List<UserDto>>();
        list!.Select(x => x.UserName).Should().Contain(new[] { "u1", "u2" });
    }

    [Fact(DisplayName = "GET /api/users/{id} 单个用户")]
    public async Task Detail_Works()
    {
        var token = await _c.RegisterAndGetTokenAsync("single", "s@mail");
        _c.UseJwt(token);

        var me = await _c.GetFromJsonAsync<UserDto>("/api/users/me");
        var resp = await _c.GetAsync($"/api/users/{me!.Id}");

        resp.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await resp.Content.ReadFromJsonAsync<UserDto>();
        dto!.UserName.Should().Be("single");
    }
}
