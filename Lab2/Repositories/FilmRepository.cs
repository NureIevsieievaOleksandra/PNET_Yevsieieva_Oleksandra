using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
using CinemaApp.Models;

namespace CinemaApp.Repositories;

public interface IFilmRepository : IRepository<Film>
{
    Task<IEnumerable<Film>> GetAllWithStudiosAsync();
    Task<Film?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Film>> SearchAsync(string? title, string? genre, int? studioId);
}

public class FilmRepository : Repository<Film>, IFilmRepository
{
    public FilmRepository(CinemaDbContext context) : base(context) { }

    public async Task<IEnumerable<Film>> GetAllWithStudiosAsync()
        => await _dbSet.Include(f => f.Studio).OrderBy(f => f.Title).ToListAsync();

    public async Task<Film?> GetByIdWithDetailsAsync(int id)
        => await _dbSet
            .Include(f => f.Studio)
            .Include(f => f.Screenings)
            .FirstOrDefaultAsync(f => f.Film_ID == id);

    public async Task<IEnumerable<Film>> SearchAsync(string? title, string? genre, int? studioId)
    {
        var query = _dbSet.Include(f => f.Studio).AsQueryable();
        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(f => f.Title.Contains(title));
        if (!string.IsNullOrWhiteSpace(genre))
            query = query.Where(f => f.Genre == genre);
        if (studioId.HasValue)
            query = query.Where(f => f.Studio_ID == studioId.Value);
        return await query.OrderBy(f => f.Title).ToListAsync();
    }
}
