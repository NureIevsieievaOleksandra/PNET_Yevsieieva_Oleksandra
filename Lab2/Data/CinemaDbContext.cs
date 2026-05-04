using Microsoft.EntityFrameworkCore;
using CinemaApp.Models;

namespace CinemaApp.Data;

public class CinemaDbContext : DbContext
{
    public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options) { }

    public DbSet<Studio> Studios { get; set; }
    public DbSet<Film> Films { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Screening> Screenings { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketsLog> TicketsLog { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Studio>(e =>
        {
            e.HasKey(s => s.Studio_ID);
            e.Property(s => s.Name).IsRequired().HasMaxLength(50);
            e.Property(s => s.Country).HasMaxLength(30);
            e.Property(s => s.Info).HasMaxLength(100);
        });

        modelBuilder.Entity<Film>(e =>
        {
            e.HasKey(f => f.Film_ID);
            e.Property(f => f.Title).IsRequired().HasMaxLength(100);
            e.Property(f => f.Genre).HasMaxLength(30);
            e.Property(f => f.Description).HasMaxLength(200);
            e.HasOne(f => f.Studio)
             .WithMany(s => s.Films)
             .HasForeignKey(f => f.Studio_ID)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Customer>(e =>
        {
            e.HasKey(c => c.Customer_ID);
            e.Property(c => c.FirstName).IsRequired().HasMaxLength(30);
            e.Property(c => c.LastName).IsRequired().HasMaxLength(30);
            e.Property(c => c.Phone).HasMaxLength(20);
            e.Property(c => c.Email).HasMaxLength(50);
            e.Ignore(c => c.FullName);
        });

        modelBuilder.Entity<Screening>(e =>
        {
            e.HasKey(s => s.Screening_ID);
            e.Property(s => s.Hall).HasMaxLength(20);
            e.Property(s => s.TicketPrice).IsRequired();
            e.Property(s => s.TotalSeats).HasDefaultValue(100);
            e.HasOne(s => s.Film)
             .WithMany(f => f.Screenings)
             .HasForeignKey(s => s.Film_ID)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Ticket>(e =>
        {
            e.HasKey(t => t.Ticket_ID);
            e.Property(t => t.Quantity).HasDefaultValue(1);
            e.HasOne(t => t.Screening)
             .WithMany(s => s.Tickets)
             .HasForeignKey(t => t.Screening_ID)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(t => t.Customer)
             .WithMany(c => c.Tickets)
             .HasForeignKey(t => t.Customer_ID)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TicketsLog>(e =>
        {
            e.HasKey(l => l.Log_ID);
            e.Property(l => l.Action).IsRequired().HasMaxLength(10);
            e.Property(l => l.ModifyDate).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Studio>().HasData(
            new Studio { Studio_ID = 1, Name = "Warner Bros", Country = "США", Founded = 1923 },
            new Studio { Studio_ID = 2, Name = "Universal", Country = "США", Founded = 1912 },
            new Studio { Studio_ID = 3, Name = "Paramount", Country = "США", Founded = 1912 },
            new Studio { Studio_ID = 4, Name = "Marvel Studios", Country = "США", Founded = 1996 },
            new Studio { Studio_ID = 5, Name = "A24", Country = "США", Founded = 2012 },
            new Studio { Studio_ID = 6, Name = "Film.UA", Country = "Україна", Founded = 2004 }
        );

        modelBuilder.Entity<Film>().HasData(
            new Film { Film_ID = 1, Title = "Inception", Genre = "Фантастика", ReleaseYear = 2010, Duration = 148, Rating = 8.8, Studio_ID = 1, Description = "Крадіжка ідей уві сні" },
            new Film { Film_ID = 2, Title = "The Dark Knight", Genre = "Бойовик", ReleaseYear = 2008, Duration = 152, Rating = 9.0, Studio_ID = 1, Description = "Бетмен проти Джокера" },
            new Film { Film_ID = 3, Title = "Oppenheimer", Genre = "Драма", ReleaseYear = 2023, Duration = 180, Rating = 8.6, Studio_ID = 3, Description = "Батько атомної бомби" },
            new Film { Film_ID = 4, Title = "Everything Everywhere", Genre = "Комедія", ReleaseYear = 2022, Duration = 139, Rating = 7.8, Studio_ID = 5, Description = "Мультивсесвіт" },
            new Film { Film_ID = 5, Title = "Avengers Endgame", Genre = "Бойовик", ReleaseYear = 2019, Duration = 181, Rating = 8.4, Studio_ID = 4, Description = "Кінець епохи" },
            new Film { Film_ID = 6, Title = "Interstellar", Genre = "Фантастика", ReleaseYear = 2014, Duration = 169, Rating = 8.6, Studio_ID = 1, Description = "Подорож крізь червоточину" },
            new Film { Film_ID = 7, Title = "The Matrix", Genre = "Фантастика", ReleaseYear = 1999, Duration = 136, Rating = 8.7, Studio_ID = 1, Description = "Симуляція реальності" },
            new Film { Film_ID = 8, Title = "Joker", Genre = "Драма", ReleaseYear = 2019, Duration = 122, Rating = 8.4, Studio_ID = 1, Description = "Походження Джокера" },
            new Film { Film_ID = 9, Title = "Dune", Genre = "Фантастика", ReleaseYear = 2021, Duration = 155, Rating = 8.0, Studio_ID = 1, Description = "Пустельна планета" },
            new Film { Film_ID = 10, Title = "Aquaman", Genre = "Бойовик", ReleaseYear = 2018, Duration = 143, Rating = 6.9, Studio_ID = 1, Description = "Король морів" },
            new Film { Film_ID = 11, Title = "The Conjuring", Genre = "Жахи", ReleaseYear = 2013, Duration = 112, Rating = 7.5, Studio_ID = 1, Description = "Справжня історія" },
            new Film { Film_ID = 12, Title = "Wonka", Genre = "Комедія", ReleaseYear = 2023, Duration = 116, Rating = 7.2, Studio_ID = 1, Description = "Молодий Віллі Вонка" },
            new Film { Film_ID = 13, Title = "Мирослава", Genre = "Драма", ReleaseYear = 2023, Duration = 110, Rating = 7.2, Studio_ID = 6, Description = "Українська драма" }
        );

        modelBuilder.Entity<Customer>().HasData(
            new Customer { Customer_ID = 1, FirstName = "Олег", LastName = "Іваненко", Phone = "+380501234567", Email = "oleg@mail.com" },
            new Customer { Customer_ID = 2, FirstName = "Марія", LastName = "Коваленко", Phone = "+380671234567", Email = "maria@mail.com" },
            new Customer { Customer_ID = 3, FirstName = "Іван", LastName = "Петренко", Phone = "+380931234567", Email = "ivan@mail.com" },
            new Customer { Customer_ID = 4, FirstName = "Анна", LastName = "Сидоренко", Phone = "+380661234567", Email = "anna@mail.com" },
            new Customer { Customer_ID = 5, FirstName = "Дмитро", LastName = "Бондаренко", Phone = "+380991234567", Email = "dmytro@mail.com" }
        );

        modelBuilder.Entity<Screening>().HasData(
            new Screening { Screening_ID = 1, Film_ID = 1, Hall = "Зал 1", StartTime = new DateTime(2025, 6, 1, 10, 0, 0), TicketPrice = 150.0, TotalSeats = 100 },
            new Screening { Screening_ID = 2, Film_ID = 1, Hall = "Зал 2", StartTime = new DateTime(2025, 6, 1, 14, 0, 0), TicketPrice = 180.0, TotalSeats = 80 },
            new Screening { Screening_ID = 3, Film_ID = 2, Hall = "Зал 1", StartTime = new DateTime(2025, 6, 2, 16, 0, 0), TicketPrice = 150.0, TotalSeats = 100 },
            new Screening { Screening_ID = 4, Film_ID = 3, Hall = "Зал 3", StartTime = new DateTime(2025, 6, 3, 18, 0, 0), TicketPrice = 200.0, TotalSeats = 120 },
            new Screening { Screening_ID = 5, Film_ID = 5, Hall = "Зал 2", StartTime = new DateTime(2025, 6, 4, 20, 0, 0), TicketPrice = 220.0, TotalSeats = 80 },
            new Screening { Screening_ID = 6, Film_ID = 6, Hall = "Зал 1", StartTime = new DateTime(2025, 6, 5, 12, 0, 0), TicketPrice = 120.0, TotalSeats = 100 }
        );
    }
}
