using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Dtos;
using Domain.Dtos.Request;
using Domain.Dtos.Response;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
    public async Task<ActionResult<User>> Register(UserDto request)
    {
        var user = await _authService.RegisterAsync(request);
        if (user is null)
        {
            return BadRequest("username already exists");
        }
        
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
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











