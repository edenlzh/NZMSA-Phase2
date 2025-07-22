using FluentAssertions;
using HandInHand.Tests.Helpers;
using System.Net;
using System.Net.Http.Headers;
using Xunit;

namespace HandInHand.Tests;

public class UsersAvatarDeleteTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _c;
    public UsersAvatarDeleteTests(CustomWebApplicationFactory f) => _c = f.CreateClient();

    [Fact(DisplayName = "POST /api/users/me/avatar 上传头像")]
    public async Task UploadAvatar_Works()
    {
        var token = await _c.RegisterAndGetTokenAsync("picUser", "p@u.com");
        _c.UseJwt(token);

        var img = new ByteArrayContent(new byte[] { 0xFF, 0xD8, 0xFF }); // ⬅ 最小化 JPEG header
        img.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

        using var form = new MultipartFormDataContent { { img, "file", "avatar.jpg" } };
        var resp = await _c.PostAsync("/api/users/me/avatar", form);

        resp.StatusCode.Should().Be(HttpStatusCode.Created);
        var url = await resp.Content.ReadAsStringAsync();
        url.Should().StartWith("/avatars/");
    }

    [Fact(DisplayName = "DELETE /api/users/me 删除账号")]
    public async Task DeleteMe_Works()
    {
        var token = await _c.RegisterAndGetTokenAsync("delUser", "del@u.com");
        _c.UseJwt(token);

        var del = await _c.DeleteAsync("/api/users/me");
        del.StatusCode.Should().Be(HttpStatusCode.OK);

        // 任何后续受保护请求都应 401
        var me = await _c.GetAsync("/api/users/me");
        me.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
