using FluentAssertions;
using HandInHand.Dtos;
using HandInHand.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace HandInHand.Tests;

public class SkillsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SkillsControllerTests(CustomWebApplicationFactory factory)
        => _client = factory.CreateClient();

    [Fact(DisplayName = "技能 CRUD 流程")]
    public async Task Skill_CRUD_Flow()
    {
        var token = await _client.RegisterAndGetTokenAsync("skillUser", "skill@u.com");
        _client.UseJwt(token);

        /* ---------- 新建 ---------- */
        var createDto = new SkillDto { Title = "C#", Description = "dotnet", Category = "Programming" };
        var createResp = await _client.PostAsJsonAsync("/api/skills", createDto);
        createResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await createResp.Content.ReadFromJsonAsync<SkillDto>();
        created!.Id.Should().BePositive();

        /* ---------- 更新 ---------- */
        created.Description = "dotnet 8";
        var updResp = await _client.PutAsJsonAsync($"/api/skills/{created.Id}", created);
        updResp.StatusCode.Should().Be(HttpStatusCode.OK);

        /* ---------- 查询 ---------- */
        var list = await _client.GetFromJsonAsync<List<SkillDto>>("/api/skills");
        list.Should().ContainSingle(s => s.Id == created.Id);

        /* ---------- 删除 ---------- */
        var delResp = await _client.DeleteAsync($"/api/skills/{created.Id}");
        delResp.StatusCode.Should().Be(HttpStatusCode.OK);

        (await _client.GetFromJsonAsync<List<SkillDto>>("/api/skills"))
            .Should().NotContain(s => s.Id == created.Id);
    }
}
