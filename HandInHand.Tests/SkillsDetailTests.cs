using FluentAssertions;
using HandInHand.Dtos;
using HandInHand.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace HandInHand.Tests;

public class SkillsDetailTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _c;
    public SkillsDetailTests(CustomWebApplicationFactory f) => _c = f.CreateClient();

    [Fact(DisplayName = "技能 Detail / My 分支覆盖")]
    public async Task Detail_And_My_Work()
    {
        var token = await _c.RegisterAndGetTokenAsync("skillUser", "s@u.com");
        _c.UseJwt(token);

        // Create
        var createDto = new SkillDto
        {
            Title = "C#",
            Description = "打工人",
            Category = "Programming"
        };
        var create = await _c.PostAsJsonAsync("/api/skills", createDto);
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var skill = await create.Content.ReadFromJsonAsync<SkillDto>();

        // Detail
        var detail = await _c.GetAsync($"/api/skills/{skill!.Id}");
        detail.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await detail.Content.ReadFromJsonAsync<SkillDto>();
        dto!.Title.Should().Be("C#");

        // My
        var my = await _c.GetAsync("/api/skills/my");
        my.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await my.Content.ReadFromJsonAsync<List<SkillDto>>();
        list.Should().ContainSingle(x => x.Id == skill.Id);
    }
}
