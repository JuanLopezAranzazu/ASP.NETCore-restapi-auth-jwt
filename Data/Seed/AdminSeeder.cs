using BookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Data.Seed;

public static class AdminSeeder
{
    public static async Task SeedAsync(
        AppDbContext context,
        IConfiguration config
    )
    {
        var email = config["AdminSeed:Email"];
        var password = config["AdminSeed:Password"];

        // Verificar si los datos de administrador están configurados
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
            return;

        if (await context.Users.AnyAsync(u => u.Email == email))
            return;

        // Obtener el rol de administrador
        var adminRole = await context.Roles
            .FirstAsync(r => r.Name == "Admin");

        var admin = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            RoleId = adminRole.Id
        };

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}
