using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Customers;

public class DeleteModel : PageModel
{
    private readonly IRepository<Customer> _repo;
    private readonly ILogger<DeleteModel> _logger;
    public DeleteModel(IRepository<Customer> repo, ILogger<DeleteModel> logger) { _repo = repo; _logger = logger; }
    [BindProperty] public Customer Customer { get; set; } = null!;
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c == null) return NotFound();
        Customer = c;
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c != null)
        {
            await _repo.DeleteAsync(id);
            _logger.LogInformation("Customer deleted: {Name}", c.FullName);
            TempData["Success"] = $"Клієнта видалено.";
        }
        return RedirectToPage("Index");
    }
}
