// Dtos/UserUpdateDto.cs
public record UserUpdateDto(
    string? UserName,
    string? Email,
    string? OldPassword,
    string? NewPassword,
    string? AvatarUrl
);
