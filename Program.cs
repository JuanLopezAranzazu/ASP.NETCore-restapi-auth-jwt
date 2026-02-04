using Microsoft.EntityFrameworkCore;
using BookingApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios en el contenedor
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("Postgres");
    options.UseNpgsql(cs);
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Probar conexión a la base de datos
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.OpenConnection();
        dbContext.Database.CloseConnection();
        Console.WriteLine("Conexión a PostgreSQL exitosa.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al conectar a la DB: {ex.Message}");
    }
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
