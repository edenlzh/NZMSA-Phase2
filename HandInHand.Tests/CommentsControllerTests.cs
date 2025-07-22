using FluentAssertions;
using HandInHand.Dtos;
using HandInHand.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace HandInHand.Tests;

public class CommentsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CommentsControllerTests(CustomWebApplicationFactory factory)
        => _client = factory.CreateClient();

    [Fact(DisplayName = "技能评论 创建 + 查询")]
    public async Task Skill_Comment_Flow()
    {
        // 1. 用户 & 技能
        var token = await _client.RegisterAndGetTokenAsync("cmUser", "cm@u.com");
        _client.UseJwt(token);

        var skillDto = new SkillDto { Title = "Java", Description = "语言", Category = "Programming" };
        var skillResp = await _client.PostAsJsonAsync("/api/skills", skillDto);
        var skill = await skillResp.Content.ReadFromJsonAsync<SkillDto>();

        // 2. 创建评论
        var cmCreate = new CommentCreateDto { Content = "很好！" };
        var cmResp = await _client.PostAsJsonAsync($"/api/comments?skillId={skill!.Id}", cmCreate);
        cmResp.StatusCode.Should().Be(HttpStatusCode.Created);

        // 3. 列出评论
        var list = await _client.GetFromJsonAsync<List<CommentDto>>($"/api/comments?skillId={skill.Id}");
        list.Should().ContainSingle(c => c.Content == "很好！");
    }

    [Fact(DisplayName = "未带 skillId / helpRequestId 创建评论应 400")]
    public async Task Create_Comment_Without_Target_Should_BadRequest()
    {
        var token = await _client.RegisterAndGetTokenAsync("cmErr", "cmerr@u.com");
        _client.UseJwt(token);

        var resp = await _client.PostAsJsonAsync("/api/comments", new CommentCreateDto { Content = "Oops" });
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
