namespace BookingApi.DTOs.Resources;

public class ResourceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Capacity { get; set; }
}