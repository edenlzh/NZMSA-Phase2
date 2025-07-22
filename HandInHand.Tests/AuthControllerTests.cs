using FluentAssertions;
using HandInHand.Dtos;
using HandInHand.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace HandInHand.Tests;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthControllerTests(CustomWebApplicationFactory factory)
        => _client = factory.CreateClient();

    [Fact(DisplayName = "注册 + 登录 正常流程")]
    public async Task Register_Then_Login_Works()
    {
        // 注册
        var regDto = new RegisterDto("userA", "a@test.com", "Abc123!");
        var regResp = await _client.PostAsJsonAsync("/api/auth/register", regDto);
        regResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var regPayload = await regResp.Content.ReadFromJsonAsync<AuthResponseDto>();
        regPayload!.Token.Should().NotBeNullOrEmpty();

        // 登录
        var loginDto = new LoginDto(regDto.UserName, regDto.Password);
        var loginResp = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
        loginResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginPayload = await loginResp.Content.ReadFromJsonAsync<AuthResponseDto>();
        loginPayload!.Token.Should().NotBeNullOrEmpty();
        // 两个 Token 可能因签发时间不同而不一致，仅验证载荷一致性
        var regSub   = JwtHelper.GetClaim(regPayload.Token!, "sub");
        var loginSub = JwtHelper.GetClaim(loginPayload.Token!, "sub");
        regSub.Should().Be(loginSub);
    }

    [Fact(DisplayName = "重复用户名注册应返回 400")]
    public async Task Duplicate_Register_Should_Fail()
    {
        var dto = new RegisterDto("dupUser", "dup@a.com", "Abc123!");
        (await _client.PostAsJsonAsync("/api/auth/register", dto)).EnsureSuccessStatusCode();

        var resp = await _client.PostAsJsonAsync("/api/auth/register", dto);
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
