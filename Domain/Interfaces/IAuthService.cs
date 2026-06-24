using Application.DTOs.Request;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Models;

namespace Domain.Interfaces;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserRegisterDto request);
    Task<TokenResponseDto> LoginAsync(UserLoginDto request);
    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request); 
}