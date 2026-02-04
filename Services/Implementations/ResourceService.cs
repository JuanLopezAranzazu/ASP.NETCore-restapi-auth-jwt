namespace BookingApi.Services.Implementations;

using Microsoft.EntityFrameworkCore;
using BookingApi.Data;
using BookingApi.Services.Interfaces;
using BookingApi.Models;
using BookingApi.DTOs.Resources;

public class ResourceService : IResourceService
{
    private readonly AppDbContext _context;

    public ResourceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ResourceDto>> GetAllAsync()
    {   
        // Obtener todos los recursos
        return await _context.Resources
            .Select(r => new ResourceDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Capacity = r.Capacity,
            })
            .ToListAsync();
    }

    public async Task<ResourceDto> GetByIdAsync(int id)
    {
        // Verificar si el recurso existe
        var resource = await _context.Resources.FindAsync(id)
            ?? throw new NotFoundException("Recurso no encontrado");

        return new ResourceDto
        {
            Id = resource.Id,
            Name = resource.Name,
            Description = resource.Description,
            Capacity = resource.Capacity,
        };
    }

    public async Task<ResourceDto> CreateAsync(CreateResourceDto dto)
    {
        // Crear el nuevo recurso
        var resource = new Resource
        {
            Name = dto.Name,
            Description = dto.Description,
            Capacity = dto.Capacity,
        };

        // Guardar en la base de datos
        _context.Resources.Add(resource);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(resource.Id);
    }

    public async Task<ResourceDto> UpdateAsync(int id, UpdateResourceDto dto)
    {
        // Verificar si el recurso existe
        var resource = await _context.Resources.FindAsync(id)
            ?? throw new NotFoundException("Recurso no encontrado");

        // Actualizar los campos del recurso
        resource.Name = dto.Name;
        resource.Description = dto.Description;
        resource.Capacity = dto.Capacity;

        await _context.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        // Verificar si el recurso existe
        var resource = await _context.Resources.FindAsync(id)
            ?? throw new NotFoundException("Recurso no encontrado");

        // Verificar si el recurso tiene reservas activas
        var hasActiveBookings = await _context.Bookings.AnyAsync(b =>
        b.ResourceId == id && b.Status == "Active");

        if (hasActiveBookings)
            throw new ConflictException("No se puede eliminar el recurso con reservas activas");

        // Eliminar el recurso
        _context.Resources.Remove(resource);
        await _context.SaveChangesAsync();
    }
}
