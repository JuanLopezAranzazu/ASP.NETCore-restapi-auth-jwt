using Microsoft.AspNetCore.Mvc;
using BookingApi.DTOs.Bookings;
using BookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BookingApi.Controllers;

[Authorize]
[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _service;

    public BookingsController(IBookingService service)
    {
        _service = service;
    }

    private int UserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private bool IsAdmin =>
        User.IsInRole("Admin");

    [HttpGet]
    public async Task<IActionResult> GetMine()
        => Ok(await _service.GetByUserAsync(UserId));

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _service.GetByIdAsync(id, UserId, IsAdmin));

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingDto dto)
    {
        var createdBooking = await _service.CreateAsync(UserId, dto);
        return CreatedAtAction(nameof(GetMine), new { id = createdBooking.Id }, createdBooking);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        await _service.CancelAsync(id, UserId, IsAdmin);
        return NoContent();
    }
}
