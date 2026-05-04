using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CinemaApp.Models;
using CinemaApp.Repositories;

namespace CinemaApp.Pages.Films;

public class IndexModel : PageModel
{
    private readonly IFilmRepository _filmRepo;
    private readonly IRepository<Studio> _studioRepo;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IFilmRepository filmRepo, IRepository<Studio> studioRepo, ILogger<IndexModel> logger)
    {
        _filmRepo = filmRepo;
        _studioRepo = studioRepo;
        _logger = logger;
    }

    public IEnumerable<Film> Films { get; set; } = new List<Film>();
    public SelectList? Studios { get; set; }

    [BindProperty(SupportsGet = true)] public string? SearchTitle { get; set; }
    [BindProperty(SupportsGet = true)] public string? SearchGenre { get; set; }
    [BindProperty(SupportsGet = true)] public int? SearchStudio { get; set; }

    public async Task OnGetAsync()
    {
        _logger.LogInformation("Films list requested. Filter: title={Title}, genre={Genre}", SearchTitle, SearchGenre);
        Films = await _filmRepo.SearchAsync(SearchTitle, SearchGenre, SearchStudio);
        var studios = await _studioRepo.GetAllAsync();
        Studios = new SelectList(studios, "Studio_ID", "Name");
    }
}
