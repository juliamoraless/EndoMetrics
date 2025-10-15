using Domain.Dtos;
using Domain.Dtos.Request;
using Domain.Dtos.Response;
using Domain.Models;

namespace Domain.Interfaces;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserDto request);
    Task<TokenResponseDto> LoginAsync(UserDto request);
    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request); 
}