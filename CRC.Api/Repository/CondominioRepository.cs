using CRC.Data;
using CRC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRC.Api.Repository;

public class CondominioRepository : IRepository<Condominio>
{
    private readonly CrcDbContext _context;
    private readonly DbSet<Condominio> _db;
    
    public CondominioRepository(CrcDbContext db)
    {
        this._context = db;
        this._db = db.Set<Condominio>();
    }
    
    public async Task<IEnumerable<Condominio>> GetAllAsync(int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;
        return _db.Skip(skip).Take(pageSize).ToList();
    }

    public async Task<Condominio> GetByIdAsync(int id)
    {
        return _db.Find(id);
    }

    public async Task<Condominio> AddAsync(Condominio entity)
    {
        await _db.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Condominio> UpdateAsync(Condominio entity)
    {
        _db.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Condominio> DeleteAsync(Condominio entity)
    {
        
        // remove cascade
        _db.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    
}