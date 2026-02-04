using Microsoft.EntityFrameworkCore;
using BookingApi.Models;

namespace BookingApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable(tb =>
            {
                tb.HasCheckConstraint(
                    "CK_Booking_DateRange",
                    "\"EndDate\" > \"StartDate\""
                );
            });
        });

        modelBuilder.Entity<Booking>()
            .Property(b => b.StartDate)
            .HasColumnType("timestamp with time zone");

        modelBuilder.Entity<Booking>()
            .Property(b => b.EndDate)
            .HasColumnType("timestamp with time zone");
    }
}