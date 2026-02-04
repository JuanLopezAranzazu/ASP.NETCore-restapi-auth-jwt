namespace BookingApi.DTOs.Bookings;

public class BookingDto
{
    public int Id { get; set; }
    public string ResourceName { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = "";
}
