using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
using CinemaApp.Models;

namespace CinemaApp.Pages.Customers;

public class IndexModel : PageModel
{
    private readonly CinemaDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(CinemaDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IEnumerable<Customer> Customers { get; set; } = new List<Customer>();

    [BindProperty(SupportsGet = true)] public string? SearchName { get; set; }
    [BindProperty(SupportsGet = true)] public string? SearchEmail { get; set; }
    [BindProperty(SupportsGet = true)] public string? SearchPhone { get; set; }

    public async Task OnGetAsync()
    {
        var query = _context.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(SearchName))
            query = query.Where(c =>
                c.FirstName.Contains(SearchName) ||
                c.LastName.Contains(SearchName));
        if (!string.IsNullOrWhiteSpace(SearchEmail))
            query = query.Where(c => c.Email != null && c.Email.Contains(SearchEmail));
        if (!string.IsNullOrWhiteSpace(SearchPhone))
            query = query.Where(c => c.Phone != null && c.Phone.Contains(SearchPhone));

        Customers = await query.OrderBy(c => c.LastName).ToListAsync();
        _logger.LogInformation("Customers filtered, count={Count}", Customers.Count());
    }
}