using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Tickets;

public class DeleteModel : PageModel
{
    private readonly ITicketRepository _repo;
    private readonly ILogger<DeleteModel> _logger;
    public DeleteModel(ITicketRepository repo, ILogger<DeleteModel> logger) { _repo = repo; _logger = logger; }

    [BindProperty] public Ticket Ticket { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var t = await _repo.GetByIdWithDetailsAsync(id);
        if (t == null) return NotFound();
        Ticket = t;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await _repo.DeleteAsync(id);
        _logger.LogInformation("Ticket deleted ID={Id}", id);
        TempData["Success"] = "Квиток видалено.";
        return RedirectToPage("Index");
    }
}
