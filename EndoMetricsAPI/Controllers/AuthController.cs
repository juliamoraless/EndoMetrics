using Application.DTOs.Request;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EndoMetricsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    public IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserRegisterDto request)
    {
        var user = await _authService.RegisterAsync(request);
        if (user is null)
        {
            return BadRequest("username already exists");
        }
        
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(UserLoginDto request)
    {
        var result = await _authService.LoginAsync(request);
        
        if (result is null)
        {
            return BadRequest("username or password incorrect");
        }
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult AuthenticatedOnly()
    {
        return Ok("you are authenticated!");
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponseDto>> Refreshtoken(RefreshTokenRequestDto request)
    {
        var result = await _authService.RefreshTokensAsync(request);
        if (result is null || result.AccessToken is null || result.RefreshToken is null)
            return Unauthorized("Invalid refresh token");

        return Ok(result);
    }
}











