using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;

namespace CinemaApp.Pages;

public class IndexModel : PageModel
{
    private readonly CinemaDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(CinemaDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public int FilmsCount { get; set; }
    public int StudiosCount { get; set; }
    public int ScreeningsCount { get; set; }
    public int TicketsCount { get; set; }
    public int CustomersCount { get; set; }
    public double TotalRevenue { get; set; }
    public List<(string Title, int Count)> TopFilms { get; set; } = new();

    public async Task OnGetAsync()
    {
        _logger.LogInformation("Dashboard loaded at {Time}", DateTime.Now);

        FilmsCount = await _context.Films.CountAsync();
        StudiosCount = await _context.Studios.CountAsync();
        ScreeningsCount = await _context.Screenings.CountAsync();
        TicketsCount = await _context.Tickets.SumAsync(t => (int?)t.Quantity) ?? 0;
        CustomersCount = await _context.Customers.CountAsync();

        TotalRevenue = await _context.Tickets
            .Include(t => t.Screening)
            .SumAsync(t => t.Quantity * t.Screening!.TicketPrice);

        TopFilms = await _context.Tickets
            .Include(t => t.Screening).ThenInclude(s => s!.Film)
            .GroupBy(t => t.Screening!.Film!.Title)
            .Select(g => new { Title = g.Key, Count = g.Sum(t => t.Quantity) })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .Select(x => new ValueTuple<string, int>(x.Title, x.Count))
            .ToListAsync();
    }
}
