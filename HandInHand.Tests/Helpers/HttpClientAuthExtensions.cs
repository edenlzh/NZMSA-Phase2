using HandInHand.Dtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HandInHand.Tests.Helpers;

/// <summary>
/// 常用测试辅助方法
/// </summary>
public static class HttpClientAuthExtensions
{
    /// <summary>注册并返回 JWT</summary>
    public static async Task<string> RegisterAndGetTokenAsync(
        this HttpClient client,
        string userName,
        string email,
        string password = "Abc123!")
    {
        var reg = new RegisterDto(userName, email, password);
        var resp = await client.PostAsJsonAsync("/api/auth/register", reg);
        resp.EnsureSuccessStatusCode();

        var dto = await resp.Content.ReadFromJsonAsync<AuthResponseDto>()
                  ?? throw new InvalidOperationException("Empty auth response");
        return dto.Token;
    }

    /// <summary>把 JWT 添加到请求头</summary>
    public static void UseJwt(this HttpClient client, string token)
        => client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
}
