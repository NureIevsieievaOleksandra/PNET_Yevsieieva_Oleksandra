using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Customers;

public class EditModel : PageModel
{
    private readonly IRepository<Customer> _repo;
    private readonly ILogger<EditModel> _logger;
    public EditModel(IRepository<Customer> repo, ILogger<EditModel> logger) { _repo = repo; _logger = logger; }
    [BindProperty] public Customer Customer { get; set; } = null!;
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c == null) return NotFound();
        Customer = c;
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        await _repo.UpdateAsync(Customer);
        _logger.LogInformation("Customer updated: {Name}", Customer.FullName);
        TempData["Success"] = $"Дані «{Customer.FullName}» оновлено!";
        return RedirectToPage("Index");
    }
}
