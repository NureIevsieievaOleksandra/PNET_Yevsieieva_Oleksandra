using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Screenings;

public class DeleteModel : PageModel
{
    private readonly IScreeningRepository _repo;
    private readonly ILogger<DeleteModel> _logger;
    public DeleteModel(IScreeningRepository repo, ILogger<DeleteModel> logger) { _repo = repo; _logger = logger; }

    [BindProperty] public Screening Screening { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var s = await _repo.GetByIdWithDetailsAsync(id);
        if (s == null) return NotFound();
        Screening = s;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await _repo.DeleteAsync(id);
        _logger.LogInformation("Screening deleted ID={Id}", id);
        TempData["Success"] = "Сеанс видалено.";
        return RedirectToPage("Index");
    }
}
