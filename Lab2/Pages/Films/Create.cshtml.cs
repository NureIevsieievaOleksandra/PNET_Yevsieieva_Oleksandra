using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Films;

public class CreateModel : PageModel
{
    private readonly IFilmRepository _filmRepo;
    private readonly IRepository<Studio> _studioRepo;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IFilmRepository filmRepo, IRepository<Studio> studioRepo, ILogger<CreateModel> logger)
    {
        _filmRepo = filmRepo;
        _studioRepo = studioRepo;
        _logger = logger;
    }

    [BindProperty]
    public Film Film { get; set; } = new Film();

    public SelectList? Studios { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
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

        await _filmRepo.AddAsync(Film);
        _logger.LogInformation("Film created: {Title} (ID={Id})", Film.Title, Film.Film_ID);
        TempData["Success"] = $"Фільм «{Film.Title}» успішно додано!";
        return RedirectToPage("Index");
    }

    private async Task LoadStudiosAsync()
    {
        var studios = await _studioRepo.GetAllAsync();
        Studios = new SelectList(studios, "Studio_ID", "Name");
    }
}
