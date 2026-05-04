using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
using CinemaApp.Models;

namespace CinemaApp.Pages.Tickets;

public class IndexModel : PageModel
{
    private readonly CinemaDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(CinemaDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IEnumerable<Ticket> Tickets { get; set; } = new List<Ticket>();
    public SelectList? Customers { get; set; }
    public SelectList? Films { get; set; }

    [BindProperty(SupportsGet = true)] public int? SearchCustomerId { get; set; }
    [BindProperty(SupportsGet = true)] public int? SearchFilmId { get; set; }
    [BindProperty(SupportsGet = true)] public DateOnly? SearchDate { get; set; }

    public async Task OnGetAsync()
    {
        var query = _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Screening).ThenInclude(s => s!.Film)
            .AsQueryable();

        if (SearchCustomerId.HasValue)
            query = query.Where(t => t.Customer_ID == SearchCustomerId.Value);
        if (SearchFilmId.HasValue)
            query = query.Where(t => t.Screening!.Film_ID == SearchFilmId.Value);
        if (SearchDate.HasValue)
            query = query.Where(t => t.PurchaseDate == SearchDate.Value);

        Tickets = await query.OrderByDescending(t => t.PurchaseDate).ToListAsync();

        var customers = await _context.Customers.OrderBy(c => c.LastName).ToListAsync();
        Customers = new SelectList(
            customers.Select(c => new { c.Customer_ID, Label = $"{c.FirstName} {c.LastName}" }),
            "Customer_ID", "Label");

        var films = await _context.Films.OrderBy(f => f.Title).ToListAsync();
        Films = new SelectList(films, "Film_ID", "Title");

        _logger.LogInformation("Tickets filtered, count={Count}", Tickets.Count());
    }
}