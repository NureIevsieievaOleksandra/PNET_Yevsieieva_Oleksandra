using Microsoft.EntityFrameworkCore;
using Serilog;
using CinemaApp.Data;
using CinemaApp.Repositories;
using CinemaApp.Models;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/cinema-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Use Serilog
builder.Host.UseSerilog();

// Add Razor Pages
builder.Services.AddRazorPages();

// EF
builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(3)
    )
);

// Dependency Injection
builder.Services.AddScoped<IRepository<Studio>, Repository<Studio>>();
builder.Services.AddScoped<IFilmRepository, FilmRepository>();
builder.Services.AddScoped<IScreeningRepository, ScreeningRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IRepository<Customer>, Repository<Customer>>();
builder.Services.AddScoped<IRepository<TicketsLog>, Repository<TicketsLog>>();

var app = builder.Build();

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        db.Database.Migrate();
        logger.LogInformation("Database migrations applied successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error applying migrations");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Serilog request logging
app.UseSerilogRequestLogging();

app.MapRazorPages();

app.Run();
