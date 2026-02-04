namespace BookingApi.DTOs.Bookings;

public class CreateBookingDto
{
    public int ResourceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
