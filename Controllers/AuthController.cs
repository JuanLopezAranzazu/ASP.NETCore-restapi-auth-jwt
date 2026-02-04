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
    public async Task<AuthResponseDto> Register(RegisterDto dto)
        => await _service.RegisterAsync(dto);


    [HttpPost("login")]
    public async Task<AuthResponseDto> Login(LoginDto dto)
        =>  await _service.LoginAsync(dto);

    [HttpPost("refresh")]
    public async Task<AuthResponseDto> Refresh(RefreshTokenDto dto)
        => await _service.RefreshAsync(dto.RefreshToken);

}
