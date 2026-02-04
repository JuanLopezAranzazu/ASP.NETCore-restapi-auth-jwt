using BookingApi.DTOs.Bookings;

namespace BookingApi.Services.Interfaces;

public interface IBookingService
{
    Task<IEnumerable<BookingDto>> GetAllAsync();
    Task<IEnumerable<BookingDto>> GetByUserAsync(int userId);
    Task<BookingDto> GetByIdAsync(int id, int userId, bool isAdmin);
    Task<BookingDto> CreateAsync(int userId, CreateBookingDto dto);
    Task CancelAsync(int bookingId, int userId, bool isAdmin);
}
