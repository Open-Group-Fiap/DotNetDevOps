using CRC.Data;
using CRC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRC.Api.Repository;

public class FaturaRepository : IRepository<Fatura>
{
    private readonly CrcDbContext _context;
    private readonly DbSet<Fatura> _db;
    
    public FaturaRepository(CrcDbContext db)
    {
        this._context = db;
        this._db = db.Set<Fatura>();
    }

    public async Task<IEnumerable<Fatura>> GetAllAsync(int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;
        return _db.Skip(skip).Take(pageSize).ToList();
    }

    public async Task<Fatura> GetByIdAsync(int id)
    {
        return _db.Find(id);
    }

    public async Task<Fatura> AddAsync(Fatura entity)
    {
        
        await _db.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Fatura> UpdateAsync(Fatura entity)
    {
        _db.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Fatura> DeleteAsync(Fatura entity)
    {
        // remove cascade
        _db.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    
    public async Task<IEnumerable<Fatura>> GetByMoradorAsync(int idMorador, int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;
        return _db.Where(f => f.IdMorador == idMorador).Skip(skip).Take(pageSize).ToList();
    }
}