using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Screenings;

public class DetailsModel : PageModel
{
    private readonly IScreeningRepository _repo;
    public DetailsModel(IScreeningRepository repo) { _repo = repo; }
    public Screening Screening { get; set; } = null!;
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var s = await _repo.GetByIdWithDetailsAsync(id);
        if (s == null) return NotFound();
        Screening = s;
        return Page();
    }
}
