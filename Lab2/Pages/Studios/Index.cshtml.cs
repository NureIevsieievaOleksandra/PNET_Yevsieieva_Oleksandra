using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
using CinemaApp.Models;

namespace CinemaApp.Pages.Studios;

public class IndexModel : PageModel
{
    private readonly CinemaDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(CinemaDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IEnumerable<Studio> Studios { get; set; } = new List<Studio>();

    [BindProperty(SupportsGet = true)] public string? SearchName { get; set; }
    [BindProperty(SupportsGet = true)] public string? SearchCountry { get; set; }

    public async Task OnGetAsync()
    {
        var query = _context.Studios.AsQueryable();

        if (!string.IsNullOrWhiteSpace(SearchName))
            query = query.Where(s => s.Name.Contains(SearchName));
        if (!string.IsNullOrWhiteSpace(SearchCountry))
            query = query.Where(s => s.Country != null && s.Country.Contains(SearchCountry));

        Studios = await query.OrderBy(s => s.Name).ToListAsync();
        _logger.LogInformation("Studios filtered: name={Name}, country={Country}", SearchName, SearchCountry);
    }
}