using FluentAssertions;
using HandInHand.Dtos;
using HandInHand.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace HandInHand.Tests;

public class HelpRequestsDetailTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _c;
    public HelpRequestsDetailTests(CustomWebApplicationFactory f) => _c = f.CreateClient();

    [Fact(DisplayName = "HelpRequest Detail / My / Delete 覆盖")]
    public async Task Flow_Works()
    {
        var token = await _c.RegisterAndGetTokenAsync("hrUser", "h@u.com");
        _c.UseJwt(token);

        var createDto = new HelpRequestDto
        {
            Title       = "借把螺丝刀",
            Description = "家里装灯"
        };
        var create = await _c.PostAsJsonAsync("/api/helprequests", createDto);
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var hr = await create.Content.ReadFromJsonAsync<HelpRequestDto>();

        // Detail
        var detail = await _c.GetAsync($"/api/helprequests/{hr!.Id}");
        detail.StatusCode.Should().Be(HttpStatusCode.OK);

        // My
        var my = await _c.GetAsync("/api/helprequests/my");
        my.StatusCode.Should().Be(HttpStatusCode.OK);

        // Delete
        var del = await _c.DeleteAsync($"/api/helprequests/{hr.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
