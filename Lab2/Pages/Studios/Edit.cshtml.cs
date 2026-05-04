using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Studios;

public class EditModel : PageModel
{
    private readonly IRepository<Studio> _repo;
    private readonly ILogger<EditModel> _logger;
    public EditModel(IRepository<Studio> repo, ILogger<EditModel> logger) { _repo = repo; _logger = logger; }
    [BindProperty] public Studio Studio { get; set; } = null!;
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var s = await _repo.GetByIdAsync(id);
        if (s == null) return NotFound();
        Studio = s;
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        await _repo.UpdateAsync(Studio);
        _logger.LogInformation("Studio updated: {Name}", Studio.Name);
        TempData["Success"] = $"Студію «{Studio.Name}» оновлено!";
        return RedirectToPage("Index");
    }
}
