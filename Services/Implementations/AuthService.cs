using Microsoft.EntityFrameworkCore;
using BookingApi.Data;
using BookingApi.Services.Interfaces;
using BookingApi.Models;
using BookingApi.DTOs.Auth;

namespace BookingApi.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthService(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        // Verificar si el usuario ya existe
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            throw new ConflictException("Usuario ya existe");

        var role = await _context.Roles
        .FirstAsync(r => r.Name == "User");

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            RoleId = role.Id
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        user.Role = role;

        return await GenerateAuthResponse(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        // Buscar el usuario por email
        var user = await _context.Users
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null ||
            !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedException("Credenciales inválidas");

        return await GenerateAuthResponse(user);
    }

    public async Task<AuthResponseDto> RefreshAsync(string refreshToken)
    {
        // Buscar el token de refresco en la base de datos
        var token = await _context.RefreshTokens
        .Include(t => t.User)
        .ThenInclude(u => u.Role)
        .FirstOrDefaultAsync(t =>
            t.Token == refreshToken &&
            !t.IsRevoked &&
            t.ExpiresAt > DateTime.UtcNow
        );

        if (token == null)
            throw new UnauthorizedException("Refresh token inválido");

        token.IsRevoked = true;

        var newRefresh = _tokenService.CreateRefreshToken(token.UserId);
        _context.RefreshTokens.Add(newRefresh);

        await _context.SaveChangesAsync();

        return new AuthResponseDto
        {
            AccessToken = _tokenService.CreateAccessToken(token.User),
            RefreshToken = newRefresh.Token
        };
    }

    
    private async Task<AuthResponseDto> GenerateAuthResponse(User user)
    {
        var refresh = _tokenService.CreateRefreshToken(user.Id);
        _context.RefreshTokens.Add(refresh);
        await _context.SaveChangesAsync();

        return new AuthResponseDto
        {
            AccessToken = _tokenService.CreateAccessToken(user),
            RefreshToken = refresh.Token
        };
    }
}
