using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using HandInHand.Dtos;
using Xunit;

namespace HandInHand.Tests;

public class ApiTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    public ApiTests(CustomWebAppFactory<Program> factory) => _client = factory.CreateClient();

    /* ---------- 工具 ---------- */
    private static string Unique(string p) => $"{p}_{Guid.NewGuid():N}";

    private async Task<UserDto> AuthenticateAsync(string baseName)
    {
        var userName = Unique(baseName);
        var reg = new RegisterDto(userName, $"{userName}@mail.com", "P@ssw0rd1!");
        var resp = await _client.PostAsJsonAsync("/api/auth/register", reg);

        if (!resp.IsSuccessStatusCode)
            throw new($"Register failed {(int)resp.StatusCode}: {await resp.Content.ReadAsStringAsync()}");

        var auth = await resp.Content.ReadFromJsonAsync<AuthResponseDto>();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", auth!.Token);

        var users = await _client.GetFromJsonAsync<List<UserDto>>("/api/users");
        return users!.Single(u => u.UserName == userName);
    }

    /* ---------- Users ---------- */
    [Fact]
    public async Task Users_CRUD_Flow_Works_With_Auth()
    {
        var current = await AuthenticateAsync("user");
        _client.DefaultRequestHeaders.Authorization = null;
        (await _client.GetFromJsonAsync<List<UserDto>>("/api/users"))!
            .Should().Contain(u => u.Id == current.Id);

        await AuthenticateAsync("user_edit");
        var updated = new UserDto
        {
            Id = current.Id,
            UserName = current.UserName,
            Email = Unique("new@mail.com"),
            AvatarUrl = current.AvatarUrl
        };
        (await _client.PutAsJsonAsync($"/api/users/{current.Id}", updated))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);

        (await _client.DeleteAsync($"/api/users/{current.Id}"))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    /* ---------- Skills ---------- */
    [Fact]
    public async Task Skills_CRUD_Flow_Works_With_Auth()
    {
        var me = await AuthenticateAsync("bob");

        var skillDto = new SkillDto
        {
            Title = Unique("Guitar"),
            Description = "Acoustic",
            Category = "Music",
            UserId = me.Id
        };

        var create = await _client.PostAsJsonAsync("/api/skills", skillDto);
        if (create.StatusCode != HttpStatusCode.Created)
            throw new Xunit.Sdk.XunitException(
                $"Skill create failed {(int)create.StatusCode}: {await create.Content.ReadAsStringAsync()}");

        var created = await create.Content.ReadFromJsonAsync<SkillDto>();

        // ② 匿名查询 —— 👇 这里是新的防御式写法
        _client.DefaultRequestHeaders.Authorization = null;
        var resp = await _client.GetAsync($"/api/skills?userId={me.Id}");

        if (!resp.IsSuccessStatusCode)
            throw new Xunit.Sdk.XunitException(
                $"GET /api/skills failed {(int)resp.StatusCode}: {await resp.Content.ReadAsStringAsync()}");

        var list = await resp.Content.ReadFromJsonAsync<List<SkillDto>>();
        list!.Should().ContainSingle(s => s.Id == created!.Id);

        // ③ 删除
        await AuthenticateAsync("bob_del");
        (await _client.DeleteAsync($"/api/skills/{created!.Id}"))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    /* ---------- HelpRequests ---------- */
    [Fact]
    public async Task HelpRequests_CRUD_Flow_Works_With_Auth()
    {
        var me = await AuthenticateAsync("alice");

        var reqDto = new HelpRequestDto
        {
            Title = Unique("Need JS Help"),
            Description = null,
            CreatedAt = DateTime.UtcNow,
            IsResolved = false,
            RequesterId = me.Id
        };

        var post = await _client.PostAsJsonAsync("/api/helprequests", reqDto);
        if (post.StatusCode != HttpStatusCode.Created)
            throw new($"HelpRequest create failed {(int)post.StatusCode}: {await post.Content.ReadAsStringAsync()}");

        var created = await post.Content.ReadFromJsonAsync<HelpRequestDto>();

        (await _client.PatchAsync($"/api/helprequests/{created!.Id}", JsonContent.Create(true)))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);

        _client.DefaultRequestHeaders.Authorization = null;
        (await _client.GetAsync("/api/helprequests?page=1&pageSize=5"))
            .StatusCode.Should().Be(HttpStatusCode.OK);

        await AuthenticateAsync("alice_del");
        (await _client.DeleteAsync($"/api/helprequests/{created.Id}"))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
