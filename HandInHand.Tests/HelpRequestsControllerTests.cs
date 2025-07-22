using FluentAssertions;
using HandInHand.Dtos;
using HandInHand.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace HandInHand.Tests;

public class HelpRequestsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HelpRequestsControllerTests(CustomWebApplicationFactory factory)
        => _client = factory.CreateClient();

    [Fact(DisplayName = "求助 CRUD 流程")]
    public async Task HelpRequest_CRUD_Flow()
    {
        var token = await _client.RegisterAndGetTokenAsync("reqUser", "req@u.com");
        _client.UseJwt(token);

        /* ---------- 创建 ---------- */
        var dto = new HelpRequestDto { Title = "Need laptop", Description = "Any spare laptop?" };
        var create = await _client.PostAsJsonAsync("/api/helprequests", dto);
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await create.Content.ReadFromJsonAsync<HelpRequestDto>();
        created!.Id.Should().BePositive();

        /* ---------- 更新 ---------- */
        created.Description = "MacBook if possible";
        var upd = await _client.PutAsJsonAsync($"/api/helprequests/{created.Id}", created);
        upd.StatusCode.Should().Be(HttpStatusCode.OK);

        /* ---------- 我的求助 ---------- */
        var mine = await _client.GetFromJsonAsync<List<HelpRequestDto>>("/api/helprequests/me");
        mine.Should().ContainSingle(r => r.Id == created.Id);

        /* ---------- 删除 ---------- */
        var del = await _client.DeleteAsync($"/api/helprequests/{created.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.OK);

        (await _client.GetFromJsonAsync<List<HelpRequestDto>>("/api/helprequests"))
            .Should().NotContain(r => r.Id == created.Id);
    }
}
