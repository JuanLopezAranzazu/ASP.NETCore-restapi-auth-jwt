namespace BookingApi.Services.Interfaces;

using BookingApi.Models;

public interface ITokenService
{
    string CreateAccessToken(User user);
    RefreshToken CreateRefreshToken(int userId);
}
