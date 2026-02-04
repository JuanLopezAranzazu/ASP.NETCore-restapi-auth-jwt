namespace BookingApi.Models;

public class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;


    public List<Booking> Bookings { get; set; } = new();

    public List<RefreshToken> RefreshTokens { get; set; } = new();
}
