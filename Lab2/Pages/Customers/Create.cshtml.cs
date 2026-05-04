using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Customers;

public class CreateModel : PageModel
{
    private readonly IRepository<Customer> _repo;
    private readonly ILogger<CreateModel> _logger;
    public CreateModel(IRepository<Customer> repo, ILogger<CreateModel> logger) { _repo = repo; _logger = logger; }
    [BindProperty] public Customer Customer { get; set; } = new Customer();
    public IActionResult OnGet() => Page();
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        await _repo.AddAsync(Customer);
        _logger.LogInformation("Customer created: {Name}", Customer.FullName);
        TempData["Success"] = $"Клієнта «{Customer.FullName}» додано!";
        return RedirectToPage("Index");
    }
}
