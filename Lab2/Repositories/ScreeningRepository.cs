using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
using CinemaApp.Models;

namespace CinemaApp.Repositories;

public interface IScreeningRepository : IRepository<Screening>
{
    Task<IEnumerable<Screening>> GetAllWithFilmsAsync();
    Task<Screening?> GetByIdWithDetailsAsync(int id);
}

public class ScreeningRepository : Repository<Screening>, IScreeningRepository
{
    public ScreeningRepository(CinemaDbContext context) : base(context) { }

    public async Task<IEnumerable<Screening>> GetAllWithFilmsAsync()
        => await _dbSet
            .Include(s => s.Film).ThenInclude(f => f!.Studio)
            .OrderBy(s => s.StartTime)
            .ToListAsync();

    public async Task<Screening?> GetByIdWithDetailsAsync(int id)
        => await _dbSet
            .Include(s => s.Film).ThenInclude(f => f!.Studio)
            .Include(s => s.Tickets).ThenInclude(t => t.Customer)
            .FirstOrDefaultAsync(s => s.Screening_ID == id);
}
