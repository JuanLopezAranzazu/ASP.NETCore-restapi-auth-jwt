namespace BookingApi.DTOs.Resources;

public class UpdateResourceDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Capacity { get; set; }
}
