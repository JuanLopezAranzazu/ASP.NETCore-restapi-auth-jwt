namespace BookingApi.Models;

public class Resource
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public int Capacity { get; set; }

    public List<Booking> Bookings { get; set; } = new();
}
