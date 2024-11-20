using CRC.Api.Models.Response;
using CRC.Data;
using CRC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRC.Api.Repository;

public class MoradorBonusRepository : IMoradorBonusRepository
{
    private readonly CrcDbContext _context;
    private readonly DbSet<MoradorBonus> _db;
    
    public MoradorBonusRepository(CrcDbContext db)
    {
        this._context = db;
        this._db = db.Set<MoradorBonus>();
    }
    
    
    public async Task<IEnumerable<MoradorBonus>> GetAllAsync(int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;
        return _db.Skip(skip).Take(pageSize).ToList();
    }

    public async Task<MoradorBonus> GetByIdAsync(int id)
    {
        return _db.Include(mb=> mb.Bonus).Include(mb=>mb.Morador).FirstOrDefault(x => x.Id == id);
    }

    public async Task<MoradorBonus> AddAsync(MoradorBonus entity)
    {
        await _db.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<MoradorBonus> UpdateAsync(MoradorBonus entity)
    {
        _db.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<MoradorBonus> DeleteAsync(MoradorBonus entity)
    {
        _db.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    
    public async Task<IEnumerable<MoradorBonus>> GetByMoradorIdAsync(int id)
    {
        return _db.Include(mb=> mb.Bonus).Include(mb=>mb.Morador).Where(x => x.IdMorador == id).ToList();
    }
    
    public async Task<IEnumerable<MoradorBonus>> GetByBonusIdAsync(int id)
    {
        return _db.Include(mb=> mb.Bonus).Include(mb=>mb.Morador).Where(x => x.IdBonus == id).ToList();
    }
    
    public async Task<MoradorBonus> GetByMoradorIdAndBonusIdAsync(int idMorador, int idBonus)
    {
        return _db.Include(mb=> mb.Bonus).Include(mb=>mb.Morador).FirstOrDefault(x => x.IdMorador == idMorador && x.IdBonus == idBonus);
    }
    
    public async Task<int> GetAvaliableByIdAsync(int idBonus)
    {
        return _db.Where(x => x.IdBonus == idBonus).Sum(x => x.Qtd);
    }
    
}