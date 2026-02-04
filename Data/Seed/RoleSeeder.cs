using BookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Data.Seed;

public static class RoleSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Verificar si ya existen roles
        if (await context.Roles.AnyAsync())
            return;

        // Crear roles predeterminados
        var roles = new List<Role>
        {
            new() { Name = "Admin" },
            new() { Name = "User" },
        };

        context.Roles.AddRange(roles);
        await context.SaveChangesAsync();
    }
}
