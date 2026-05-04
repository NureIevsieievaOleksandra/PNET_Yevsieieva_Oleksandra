using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Screenings;

public class CreateModel : PageModel
{
    private readonly IScreeningRepository _repo;
    private readonly IFilmRepository _filmRepo;
    private readonly ILogger<CreateModel> _logger;
    public CreateModel(IScreeningRepository repo, IFilmRepository filmRepo, ILogger<CreateModel> logger)
    { _repo = repo; _filmRepo = filmRepo; _logger = logger; }

    [BindProperty] public Screening Screening { get; set; } = new Screening { StartTime = DateTime.Now.AddDays(1), TotalSeats = 100 };
    public SelectList? Films { get; set; }

    public async Task<IActionResult> OnGetAsync() { await LoadFilmsAsync(); return Page(); }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Screening.StartTime < DateTime.Now)
            ModelState.AddModelError("Screening.StartTime", "Час початку не може бути в минулому");
        if (!ModelState.IsValid) { await LoadFilmsAsync(); return Page(); }
        await _repo.AddAsync(Screening);
        _logger.LogInformation("Screening created for film ID={Id}", Screening.Film_ID);
        TempData["Success"] = "Сеанс успішно додано!";
        return RedirectToPage("Index");
    }

    private async Task LoadFilmsAsync()
    {
        var films = await _filmRepo.GetAllWithStudiosAsync();
        Films = new SelectList(films, "Film_ID", "Title");
    }
}
