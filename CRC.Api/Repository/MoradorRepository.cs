using CRC.Data;
using CRC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRC.Api.Repository;

public class MoradorRepository : IRepository<Morador>
{
    private readonly CrcDbContext _context;
    private readonly DbSet<Morador> _db;
    
    public MoradorRepository(CrcDbContext db)
    {
        this._context = db;
        this._db = db.Set<Morador>();
    }
    
    public Task<IEnumerable<Morador>> GetAllAsync(int pageNumber, int pageSize)
    {
        var moradores = _db.Include(m=> m.Auth).Include(m=>m.Condominio).Skip((pageNumber - 1) * pageSize).Take(pageSize).AsEnumerable();
        return Task.FromResult(moradores);
    }

    public async Task<Morador> GetByIdAsync(int id)
    {
        return _db.Include(m=> m.Auth).Include(m=>m.Condominio).FirstOrDefault(x => x.Id == id);
    }

    public async Task<Morador> AddAsync(Morador entity)
    {
        await _db.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Morador> UpdateAsync(Morador entity)
    {
        _db.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Morador> DeleteAsync(Morador entity)
    {
        _db.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    
    public async Task<Morador> GetByEmailAsync(string email)
    {
        return _db.Include(m=> m.Auth).Include(m=>m.Condominio).FirstOrDefault(x => x.Auth.Email == email);
    }
    
    
    
    public async Task<Morador> GetByCpfAsync( string cpf)
    {
        return _db.Include(m=> m.Auth).Include(m=>m.Condominio).FirstOrDefault(x=> x.Cpf == cpf);
    }
    
}