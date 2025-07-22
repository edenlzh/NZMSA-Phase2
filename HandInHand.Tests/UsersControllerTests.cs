using FluentAssertions;
using HandInHand.Dtos;
using HandInHand.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace HandInHand.Tests;

public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UsersControllerTests(CustomWebApplicationFactory factory)
        => _client = factory.CreateClient();

    [Fact(DisplayName = "获取当前用户资料 /api/users/me")]
    public async Task Get_Me_Works()
    {
        var token = await _client.RegisterAndGetTokenAsync("meUser", "me@u.com");
        _client.UseJwt(token);

        var meResp = await _client.GetAsync("/api/users/me");
        meResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var me = await meResp.Content.ReadFromJsonAsync<UserDto>();
        me!.UserName.Should().Be("meUser");
        me.Email.Should().Be("me@u.com");
    }

    [Fact(DisplayName = "更新个人资料 & 重设密码")]
    public async Task Update_Me_Works()
    {
        var token = await _client.RegisterAndGetTokenAsync("updateUser", "up@u.com");
        _client.UseJwt(token);

        var upd = new UserUpdateDto(
            UserName : "newName",
            Email    : "new@mail.com",
            OldPassword : "Abc123!",
            NewPassword : "Xyz987!@",
            AvatarUrl   : null);

        var resp = await _client.PutAsJsonAsync("/api/users/me", upd);
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        // 解析并验证更新后的用户信息
        var updated = await resp.Content.ReadFromJsonAsync<UserDto>();
        updated!.UserName.Should().Be("newName");
        updated.Email.Should().Be("new@mail.com");

        // 再次登录验证密码已更新
        var login = new LoginDto("newName", "Xyz987!@");
        var loginResp = await _client.PostAsJsonAsync("/api/auth/login", login);
        loginResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
