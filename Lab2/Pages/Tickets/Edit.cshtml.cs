using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Tickets;

public class EditModel : PageModel
{
    private readonly ITicketRepository _ticketRepo;
    private readonly IScreeningRepository _screeningRepo;
    private readonly IRepository<Customer> _customerRepo;
    private readonly ILogger<EditModel> _logger;

    public EditModel(ITicketRepository ticketRepo, IScreeningRepository screeningRepo,
        IRepository<Customer> customerRepo, ILogger<EditModel> logger)
    { _ticketRepo = ticketRepo; _screeningRepo = screeningRepo; _customerRepo = customerRepo; _logger = logger; }

    [BindProperty] public Ticket Ticket { get; set; } = null!;
    public SelectList? Screenings { get; set; }
    public SelectList? Customers { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var t = await _ticketRepo.GetByIdAsync(id);
        if (t == null) return NotFound();
        Ticket = t;
        await LoadSelectsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) { await LoadSelectsAsync(); return Page(); }
        await _ticketRepo.UpdateAsync(Ticket);
        _logger.LogInformation("Ticket updated ID={Id}", Ticket.Ticket_ID);
        TempData["Success"] = "Квиток оновлено!";
        return RedirectToPage("Index");
    }

    private async Task LoadSelectsAsync()
    {
        var screenings = await _screeningRepo.GetAllWithFilmsAsync();
        Screenings = new SelectList(
            screenings.Select(s => new { s.Screening_ID, Label = $"{s.Film?.Title} | {s.Hall} | {s.StartTime:dd.MM HH:mm}" }),
            "Screening_ID", "Label");
        var customers = await _customerRepo.GetAllAsync();
        Customers = new SelectList(
            customers.Select(c => new { c.Customer_ID, Label = $"{c.FirstName} {c.LastName}" }),
            "Customer_ID", "Label");
    }
}
