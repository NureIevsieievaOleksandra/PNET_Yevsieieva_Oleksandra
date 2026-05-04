using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
using CinemaApp.Models;

namespace CinemaApp.Repositories;

public interface ITicketRepository : IRepository<Ticket>
{
    Task<IEnumerable<Ticket>> GetAllWithDetailsAsync();
    Task<Ticket?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Ticket>> GetByCustomerAsync(int customerId);
    Task<IEnumerable<Ticket>> GetByScreeningAsync(int screeningId);
}

public class TicketRepository : Repository<Ticket>, ITicketRepository
{
    public TicketRepository(CinemaDbContext context) : base(context) { }

    public async Task<IEnumerable<Ticket>> GetAllWithDetailsAsync()
        => await _dbSet
            .Include(t => t.Customer)
            .Include(t => t.Screening).ThenInclude(s => s!.Film)
            .OrderByDescending(t => t.PurchaseDate)
            .ToListAsync();

    public async Task<Ticket?> GetByIdWithDetailsAsync(int id)
        => await _dbSet
            .Include(t => t.Customer)
            .Include(t => t.Screening).ThenInclude(s => s!.Film)
            .FirstOrDefaultAsync(t => t.Ticket_ID == id);

    public async Task<IEnumerable<Ticket>> GetByCustomerAsync(int customerId)
        => await _dbSet
            .Include(t => t.Screening).ThenInclude(s => s!.Film)
            .Where(t => t.Customer_ID == customerId)
            .ToListAsync();

    public async Task<IEnumerable<Ticket>> GetByScreeningAsync(int screeningId)
        => await _dbSet
            .Include(t => t.Customer)
            .Where(t => t.Screening_ID == screeningId)
            .ToListAsync();

    public override async Task AddAsync(Ticket entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        // Log the action
        await _context.TicketsLog.AddAsync(new TicketsLog
        {
            Ticket_ID = entity.Ticket_ID,
            Action = "INSERT",
            ModifyDate = DateTime.Now
        });
        await _context.SaveChangesAsync();
    }

    public override async Task UpdateAsync(Ticket entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        await _context.TicketsLog.AddAsync(new TicketsLog
        {
            Ticket_ID = entity.Ticket_ID,
            Action = "UPDATE",
            ModifyDate = DateTime.Now
        });
        await _context.SaveChangesAsync();
    }

    public override async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            await _context.TicketsLog.AddAsync(new TicketsLog
            {
                Ticket_ID = entity.Ticket_ID,
                Action = "DELETE",
                ModifyDate = DateTime.Now
            });
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
