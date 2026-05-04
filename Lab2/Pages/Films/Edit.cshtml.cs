using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Films;

public class EditModel : PageModel
{
    private readonly IFilmRepository _filmRepo;
    private readonly IRepository<Studio> _studioRepo;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IFilmRepository filmRepo, IRepository<Studio> studioRepo, ILogger<EditModel> logger)
    {
        _filmRepo = filmRepo;
        _studioRepo = studioRepo;
        _logger = logger;
    }

    [BindProperty]
    public Film Film { get; set; } = new Film();
    public SelectList? Studios { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var film = await _filmRepo.GetByIdAsync(id);
        if (film == null) return NotFound();
        Film = film;
        await LoadStudiosAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadStudiosAsync();
            return Page();
        }

        await _filmRepo.UpdateAsync(Film);
        _logger.LogInformation("Film updated: {Title} (ID={Id})", Film.Title, Film.Film_ID);
        TempData["Success"] = $"Фільм «{Film.Title}» успішно оновлено!";
        return RedirectToPage("Index");
    }

    private async Task LoadStudiosAsync()
    {
        var studios = await _studioRepo.GetAllAsync();
        Studios = new SelectList(studios, "Studio_ID", "Name");
    }
}
