using BookingApi.DTOs.Resources;

namespace BookingApi.Services.Interfaces;

public interface IResourceService
{
    Task<List<ResourceDto>> GetAllAsync();
    Task<ResourceDto> GetByIdAsync(int id);
    Task<ResourceDto> CreateAsync(CreateResourceDto dto);
    Task<ResourceDto> UpdateAsync(int id, UpdateResourceDto dto);
    Task DeleteAsync(int id);
}
