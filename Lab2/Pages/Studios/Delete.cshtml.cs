using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Studios;

public class DeleteModel : PageModel
{
    private readonly IRepository<Studio> _repo;
    private readonly ILogger<DeleteModel> _logger;
    public DeleteModel(IRepository<Studio> repo, ILogger<DeleteModel> logger) { _repo = repo; _logger = logger; }
    [BindProperty] public Studio Studio { get; set; } = null!;
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var s = await _repo.GetByIdAsync(id);
        if (s == null) return NotFound();
        Studio = s;
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int id)
    {
        var s = await _repo.GetByIdAsync(id);
        if (s != null)
        {
            await _repo.DeleteAsync(id);
            _logger.LogInformation("Studio deleted: {Name}", s.Name);
            TempData["Success"] = $"Студію «{s.Name}» видалено.";
        }
        return RedirectToPage("Index");
    }
}
