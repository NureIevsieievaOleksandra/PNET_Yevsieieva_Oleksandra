using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Films;

public class DeleteModel : PageModel
{
    private readonly IFilmRepository _filmRepo;
    private readonly ILogger<DeleteModel> _logger;
    public DeleteModel(IFilmRepository filmRepo, ILogger<DeleteModel> logger)
    { _filmRepo = filmRepo; _logger = logger; }
    [BindProperty] public Film Film { get; set; } = null!;
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var film = await _filmRepo.GetByIdWithDetailsAsync(id);
        if (film == null) return NotFound();
        Film = film;
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int id)
    {
        var film = await _filmRepo.GetByIdAsync(id);
        if (film != null)
        {
            var title = film.Title;
            await _filmRepo.DeleteAsync(id);
            _logger.LogInformation("Film deleted: {Title} (ID={Id})", title, id);
            TempData["Success"] = $"Фільм «{title}» видалено.";
        }
        return RedirectToPage("Index");
    }
}
