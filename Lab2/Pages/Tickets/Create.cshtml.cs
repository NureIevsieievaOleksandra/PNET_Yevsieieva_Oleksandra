using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Tickets;

public class CreateModel : PageModel
{
    private readonly ITicketRepository _ticketRepo;
    private readonly IScreeningRepository _screeningRepo;
    private readonly IRepository<Customer> _customerRepo;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(ITicketRepository ticketRepo, IScreeningRepository screeningRepo,
        IRepository<Customer> customerRepo, ILogger<CreateModel> logger)
    { _ticketRepo = ticketRepo; _screeningRepo = screeningRepo; _customerRepo = customerRepo; _logger = logger; }

    [BindProperty] public Ticket Ticket { get; set; } = new Ticket { PurchaseDate = DateOnly.FromDateTime(DateTime.Today), Quantity = 1 };
    public SelectList? Screenings { get; set; }
    public SelectList? Customers { get; set; }

    public async Task<IActionResult> OnGetAsync() { await LoadSelectsAsync(); return Page(); }

    public async Task<IActionResult> OnPostAsync()
    {
        var screening = await _screeningRepo.GetByIdAsync(Ticket.Screening_ID);
        if (screening != null && screening.StartTime < DateTime.Now)
            ModelState.AddModelError("Ticket.Screening_ID", "Неможливо придбати квиток на сеанс, що вже відбувся");

        if (!ModelState.IsValid) { await LoadSelectsAsync(); return Page(); }
        await _ticketRepo.AddAsync(Ticket);
        _logger.LogInformation("Ticket sold: screening={S}, customer={C}, qty={Q}", Ticket.Screening_ID, Ticket.Customer_ID, Ticket.Quantity);
        TempData["Success"] = "Квиток успішно продано!";
        return RedirectToPage("Index");
    }

    private async Task LoadSelectsAsync()
    {
        var screenings = await _screeningRepo.GetAllWithFilmsAsync();
        Screenings = new SelectList(
            screenings.Select(s => new { s.Screening_ID, Label = $"{s.Film?.Title} | {s.Hall} | {s.StartTime:dd.MM HH:mm} | {s.TicketPrice}грн" }),
            "Screening_ID", "Label");
        var customers = await _customerRepo.GetAllAsync();
        Customers = new SelectList(
            customers.Select(c => new { c.Customer_ID, Label = $"{c.FirstName} {c.LastName}" }),
            "Customer_ID", "Label");
    }
}
