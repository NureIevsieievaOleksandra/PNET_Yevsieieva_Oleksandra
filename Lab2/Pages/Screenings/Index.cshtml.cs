using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
using CinemaApp.Models;

namespace CinemaApp.Pages.Screenings;

public class IndexModel : PageModel
{
    private readonly CinemaDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(CinemaDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IEnumerable<Screening> Screenings { get; set; } = new List<Screening>();
    public SelectList? Films { get; set; }

    [BindProperty(SupportsGet = true)] public int? SearchFilmId { get; set; }
    [BindProperty(SupportsGet = true)] public string? SearchHall { get; set; }
    [BindProperty(SupportsGet = true)] public string? SearchStatus { get; set; } // "upcoming" | "past"

    public async Task OnGetAsync()
    {
        var query = _context.Screenings
            .Include(s => s.Film).ThenInclude(f => f!.Studio)
            .AsQueryable();

        if (SearchFilmId.HasValue)
            query = query.Where(s => s.Film_ID == SearchFilmId.Value);
        if (!string.IsNullOrWhiteSpace(SearchHall))
            query = query.Where(s => s.Hall != null && s.Hall.Contains(SearchHall));
        if (SearchStatus == "upcoming")
            query = query.Where(s => s.StartTime >= DateTime.Now);
        else if (SearchStatus == "past")
            query = query.Where(s => s.StartTime < DateTime.Now);

        Screenings = await query.OrderBy(s => s.StartTime).ToListAsync();

        var films = await _context.Films.OrderBy(f => f.Title).ToListAsync();
        Films = new SelectList(films, "Film_ID", "Title");

        _logger.LogInformation("Screenings filtered, count={Count}", Screenings.Count());
    }
}