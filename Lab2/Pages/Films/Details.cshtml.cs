using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Films;

public class DetailsModel : PageModel
{
    private readonly IFilmRepository _filmRepo;
    private readonly ILogger<DetailsModel> _logger;
    public DetailsModel(IFilmRepository filmRepo, ILogger<DetailsModel> logger)
    { _filmRepo = filmRepo; _logger = logger; }
    public Film Film { get; set; } = null!;
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var film = await _filmRepo.GetByIdWithDetailsAsync(id);
        if (film == null) return NotFound();
        Film = film;
        _logger.LogInformation("Film details viewed: {Title}", film.Title);
        return Page();
    }
}
