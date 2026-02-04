using Microsoft.EntityFrameworkCore;
using BookingApi.Data;
using BookingApi.Models;
using BookingApi.DTOs.Bookings;
using BookingApi.Services.Interfaces;

namespace BookingApi.Services.Implementations;

public class BookingService : IBookingService
{
    private readonly AppDbContext _context;

    public BookingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookingDto>> GetAllAsync()
    {
        // Obtener todas las reservas
        return await _context.Bookings
            .Include(b => b.Resource)
            .Include(b => b.User)
            .OrderByDescending(b => b.StartDate)
            .Select(b => ToDto(b))
            .ToListAsync();
    }


    public async Task<IEnumerable<BookingDto>> GetByUserAsync(int userId)
    {
        // Obtener reservas por usuario
        return await _context.Bookings
            .Include(b => b.Resource)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.StartDate)
            .Select(b => ToDto(b))
            .ToListAsync();
    }

    public async Task<BookingDto> GetByIdAsync(int id, int userId, bool isAdmin)
    {
        // Obtener reserva por ID
        var booking = await _context.Bookings
            .Include(b => b.Resource)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
            throw new NotFoundException("Reserva no encontrada");

        // Verificar permisos
        if (!isAdmin && booking.UserId != userId)
            throw new ForbiddenException("No tienes acceso a esta reserva");

        return ToDto(booking);
    }

    public async Task<BookingDto> CreateAsync(int userId, CreateBookingDto dto)
    {
        var startUtc = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc);
        var endUtc = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc);

        // Verificar fechas
        if (endUtc <= startUtc)
            throw new BadRequestException("La fecha final debe ser mayor a la inicial");

        // Verificar que el recurso exista
        var resource = await _context.Resources
            .FirstOrDefaultAsync(r => r.Id == dto.ResourceId);

        if (resource == null)
            throw new BadRequestException("Recurso no existe");

        // Verificar solapamientos
        var overlap = await _context.Bookings.AnyAsync(b =>
            b.ResourceId == dto.ResourceId &&
            b.Status == "Active" &&
            startUtc < b.EndDate &&
            endUtc > b.StartDate
        );

        if (overlap)
            throw new ConflictException("El recurso ya está reservado en ese rango");

        var booking = new Booking
        {
            UserId = userId,
            ResourceId = dto.ResourceId,
            StartDate = startUtc,
            EndDate = endUtc,
            Status = "Active"
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        booking.Resource = resource;

        return ToDto(booking);
    }

    public async Task CancelAsync(int bookingId, int userId, bool isAdmin)
    {
        // Buscar reserva
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking == null)
            throw new NotFoundException("Reserva no encontrada");

        // Verificar permisos
        if (!isAdmin && booking.UserId != userId)
            throw new ForbiddenException("No puedes cancelar esta reserva");

        if (booking.Status != "Active")
            throw new ConflictException("La reserva no está activa");

        booking.Status = "Cancelled";
        await _context.SaveChangesAsync();
    }

    private static BookingDto ToDto(Booking booking)
    {
        return new BookingDto
        {
            Id = booking.Id,
            ResourceName = booking.Resource.Name,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Status = booking.Status
        };
    }
}
