using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Screenings;

public class EditModel : PageModel
{
    private readonly IScreeningRepository _repo;
    private readonly IFilmRepository _filmRepo;
    private readonly ILogger<EditModel> _logger;
    public EditModel(IScreeningRepository repo, IFilmRepository filmRepo, ILogger<EditModel> logger)
    { _repo = repo; _filmRepo = filmRepo; _logger = logger; }

    [BindProperty] public Screening Screening { get; set; } = null!;
    public SelectList? Films { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var s = await _repo.GetByIdAsync(id);
        if (s == null) return NotFound();
        Screening = s;
        await LoadFilmsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) { await LoadFilmsAsync(); return Page(); }
        await _repo.UpdateAsync(Screening);
        _logger.LogInformation("Screening updated ID={Id}", Screening.Screening_ID);
        TempData["Success"] = "Сеанс оновлено!";
        return RedirectToPage("Index");
    }

    private async Task LoadFilmsAsync()
    {
        var films = await _filmRepo.GetAllWithStudiosAsync();
        Films = new SelectList(films, "Film_ID", "Title");
    }
}
