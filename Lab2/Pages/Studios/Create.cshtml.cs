using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Studios;

public class CreateModel : PageModel
{
    private readonly IRepository<Studio> _repo;
    private readonly ILogger<CreateModel> _logger;
    public CreateModel(IRepository<Studio> repo, ILogger<CreateModel> logger) { _repo = repo; _logger = logger; }
    [BindProperty] public Studio Studio { get; set; } = new Studio();
    public IActionResult OnGet() => Page();
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        await _repo.AddAsync(Studio);
        _logger.LogInformation("Studio created: {Name}", Studio.Name);
        TempData["Success"] = $"Студію «{Studio.Name}» додано!";
        return RedirectToPage("Index");
    }
}
