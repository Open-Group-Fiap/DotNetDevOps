using CRC.Data;
using CRC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRC.Api.Repository;

public class BonusRepository : IRepository<Bonus>
{
    private readonly CrcDbContext _context;
    private readonly DbSet<Bonus> _db;
    
    public BonusRepository(CrcDbContext db)
    {
        this._context = db;
        this._db = db.Set<Bonus>();
    }
    
    
    public async Task<IEnumerable<Bonus>> GetAllAsync(int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;
        return _db.Include(b=>b.Condominio).Skip(skip).Take(pageSize).ToList();
    }

    public async Task<Bonus> GetByIdAsync(int id)
    {
        return _db.Include(b=>b.Condominio).FirstOrDefault(x => x.Id == id);
    }

    public async Task<Bonus> AddAsync(Bonus entity)
    {
        await _db.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Bonus> UpdateAsync(Bonus entity)
    {
        _db.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Bonus> DeleteAsync(Bonus entity)
    {
        _db.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    
    public async Task<IEnumerable<Bonus>> GetByCondominioIdAsync(int idCondominio)
    {
        return _db.Include(b=> b.Condominio).Where(b => b.IdCondominio == idCondominio).ToList();
    }
    
}