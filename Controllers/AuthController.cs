using Microsoft.AspNetCore.Mvc;
using BookingApi.DTOs.Auth;
using BookingApi.Services.Interfaces;

namespace BookingApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
        => Ok(await _service.RegisterAsync(dto));


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
        => Ok(await _service.LoginAsync(dto));


    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        => Ok(await _service.RefreshAsync(dto));

}
